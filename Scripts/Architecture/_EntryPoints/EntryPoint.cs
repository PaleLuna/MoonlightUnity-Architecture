using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Initializer;
using PaleLuna.Attributes;
using PaleLuna.DataHolder;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaleLuna.Architecture.EntryPoint
{
    public abstract class EntryPoint : MonoBehaviour
    {

        [Foldout("Events")]
        [SerializeField]
        protected UnityEvent _initStartEvent = new();
        [Foldout("Events")]
        [SerializeField]
        protected UnityEvent _initEndEvent = new();

        [Foldout("Events")]
        [SerializeField]
        protected UnityEvent _startingComponentsStartEvent = new();
        [Foldout("Events")]
        [SerializeField]
        protected UnityEvent _startingCompileComponentsEndEvent = new();

        private const int DEFAULT_LIST_CAPACITY = 10;

        [SerializeReference, RequireInterface(typeof(IInitializer))]
        private List<MonoBehaviour> _initializersMono = new(DEFAULT_LIST_CAPACITY);

        [Header("Startables")]
        [SerializeReference, RequireInterface(typeof(IStartable))]
        private List<MonoBehaviour> _startablesMono;

        private DataHolder<IStartable> _startables;
        protected DataHolder<IInitializer> _initializers = new(DEFAULT_LIST_CAPACITY);

        protected virtual void Awake() =>
            _ = Setup();

        protected virtual async UniTask Setup()
        {
            FillInitializers();
            CompileAllInitializers();
            StartAllInitializers();

            await LoadAllServices();

            CompileAllComponents();
            StartAllComponents();
        }

        protected virtual void FillInitializers() { }

        protected virtual void StartAllInitializers()
        {
            _initStartEvent.Invoke();

            _initializers.ForEach(initializer =>
            {
                if (initializer.status == InitStatus.Shutdown)
                    initializer.StartInit();
            });
        }

        private void CompileAllComponents()
        {
            _startables = new DataHolder<IStartable>(_startablesMono.Count);
            _startablesMono.ForEach(behaviour => _startables.Registration((IStartable)behaviour));

            _startables.Registration(
                Searcher.ListOfAllByInterface<IStartable>(item => item.IsStarted == false),
                ListRegistrationType.MergeToEndUnion
            );
        }

        private void CompileAllInitializers()
        {
            _initializersMono.ForEach(behaviour =>
                _initializers.Registration((IInitializer)behaviour)
            );
        }

        protected void StartAllComponents()
        {
            _startingComponentsStartEvent.Invoke();

            _startables.ForEach(item => item.OnStart());

            _startingCompileComponentsEndEvent.Invoke();
        }

        protected async UniTask LoadAllServices()
        {
            int currentDoneInits = 0;

            while (currentDoneInits < _initializers.Count)
            {
                int lastDoneInits = 0;

                _initializers.ForEach(item =>
                {
                    if (item.status == InitStatus.Done)
                        lastDoneInits++;
                });

                if (lastDoneInits > currentDoneInits)
                {
                    currentDoneInits = lastDoneInits;
                    print($"Loading services: {currentDoneInits} / {_initializers.Count}");
                }

                await UniTask.Yield();
            }

            _initEndEvent.Invoke();
        }
    }
}

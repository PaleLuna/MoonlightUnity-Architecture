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
        private const int DEFAULT_LIST_CAPACITY = 10;

        #region [ Propertirs ]

        #region [ Events ]

        [Foldout("Initialization events"), SerializeField]
        protected UnityEvent _initStartEvent = new();
        [Foldout("Initialization events"), SerializeField]
        protected UnityEvent _initEndEvent = new();

        [Foldout("Initialization events"), SerializeField]
        protected UnityEvent _startingComponentsStartEvent = new();
        [Foldout("Initialization events"), SerializeField]
        protected UnityEvent _startingCompileComponentsEndEvent = new();

        #endregion

        [Header("List of objects to be initialized at scene startup"), HorizontalLine(color: EColor.Violet)]
        [SerializeReference, RequireInterface(typeof(IInitializer))]
        private List<MonoBehaviour> _initializersMono = new(DEFAULT_LIST_CAPACITY);
        
        [SerializeReference, RequireInterface(typeof(IStartable))]
        private List<MonoBehaviour> _startupsMono;

        private DataHolder<IStartable> _startups;
        protected DataHolder<IInitializer> _initializers = new(DEFAULT_LIST_CAPACITY);

        [Space, SerializeField]
        private bool _clearInitializersAfterInit = false;
        #endregion

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

            Clear();
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
            _startups = new DataHolder<IStartable>(_startupsMono.Count);
            _startupsMono.ForEach(behaviour => _startups.Registration((IStartable)behaviour));

            _startups.Registration(
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

            _startups.ForEach(item => item.OnStart());

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

        protected void Clear()
        {
            _startups.Clear();
            _initializers.Clear();
            _startupsMono.Clear();

            if(_clearInitializersAfterInit)
                ClearMono(_initializersMono);
        }

        private void ClearMono(List<MonoBehaviour> behaviours)
        {
            behaviours.ForEach(item => Destroy(item));
            behaviours.Clear();
        }
    }
}

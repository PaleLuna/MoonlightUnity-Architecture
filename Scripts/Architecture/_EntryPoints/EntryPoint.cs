using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PaleLuna.Attributes;
using UnityEngine;

namespace PaleLuna.Architecture
{
    
    public abstract class EntryPoint : MonoBehaviour
    {
        private const int DEFAULT_LIST_CAPACITY = 10;
        
        protected List<IInitializer> _initializersList = new(DEFAULT_LIST_CAPACITY);
        
        [Header("Startables")] 
        [SerializeReference, RequireInterface(typeof(IStartable))]
        private List<MonoBehaviour> _startablesMono;
        
        private DataHolder<IStartable> _startables;

        protected virtual async UniTaskVoid Setup() => await LoadAllServices();

        protected abstract void FillInitializers();
        protected abstract void StartAllInitializers();
        
        protected void CompileAllComponents()
        {
            _startables = new DataHolder<IStartable>(_startablesMono.Count);
            _startablesMono.ForEach(behaviour => _startables.Registration((IStartable)behaviour));

            _startables.Registration(
                Searcher.ListOfAllByInterface<IStartable>(item => item.IsStarted == false), 
                ListRegistrationType.MergeToEndUnion);
        }
        protected void StartAllComponents() =>
            _startables.ForEach(item => item.OnStart());

        protected async UniTask LoadAllServices()
        {
            int currentDoneInits = 0;
            
            while (currentDoneInits < _initializersList.Count)
            {
                int lastDoneInits = 0;
                
                foreach (IInitializer item in _initializersList)
                    if (item.status == InitStatus.Done)
                        lastDoneInits++;

                if (lastDoneInits > currentDoneInits)
                {
                    currentDoneInits = lastDoneInits;
                    print($"Loading services: {currentDoneInits} / {_initializersList.Count}");
                }

                await UniTask.Yield();
            }
        }
    }
}
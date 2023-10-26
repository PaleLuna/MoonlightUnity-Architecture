using System;
using System.Collections.Generic;

namespace _Scripts.Other.Patterns.StatePattern
{
    public class StateHolder<T> : IStateHolder<T> where T : State
    {
        private DictionaryDataHolder<T> _stateMap;
        public T currentState { get; private set; }

        public StateHolder() => 
            _stateMap = new DictionaryDataHolder<T>();
        
        public TP Registarion<TP>(TP item) where TP : T => 
            _stateMap.Registration<TP>(item);

        public TP Unregistration<TP>() where TP : T => 
            _stateMap.Unregistration<TP>();

        public void ChangeState<TP>() where TP : T
        {
            Type type = typeof(TP);

            T newState = _stateMap.GetByType<TP>();
            
            if(newState == null) return;
            
            currentState?.StateStop();
            currentState = newState;
            currentState.StateStart();
        }
    }
}
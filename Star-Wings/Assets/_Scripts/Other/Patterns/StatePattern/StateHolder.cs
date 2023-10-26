using System;
using System.Collections.Generic;

namespace _Scripts.Other.Patterns.StatePattern
{
    public class StateHolder<T> : IStateHolder<T> where T : State
    {
        private UniqDataHolder<T> _stateMap;
        public T currentState { get; private set; }

        public StateHolder() => _stateMap = new UniqDataHolder<T>();


        public TP Registarion<TP>(TP item) where TP : T
        {
            _stateMap.Registration<TP>(item);

            return item;
        }

        public TP Unregistration<TP>(TP item) where TP : T
        {
            return _stateMap.Unregistration<TP>(item);
        }

        public void ChangeState<TP>() where TP : T
        {
            Type type = typeof(TP);

            T newState = _stateMap.GetFirstByType<TP>();
            
            if(newState == null) return;
            
            currentState?.StateStop();
            currentState = newState;
            currentState.StateStart();
        }
    }
}
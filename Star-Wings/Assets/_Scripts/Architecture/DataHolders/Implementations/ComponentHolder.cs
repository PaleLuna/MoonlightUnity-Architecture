using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace _Scripts.Architecture.DataHolders.Implementations
{
    public class ComponentHolder<T> : IComponentHolder<T> where T : IGameComponent
    {
        private UnityEvent onComponentAdded = new UnityEvent();

        private Dictionary<Type, T> _componentsMap;

        public UnityEvent OnComponentAdded => onComponentAdded;

        public TP Register<TP>(TP newComponent) where TP : T
        {
            Type type = newComponent.GetType();

            if (_componentsMap.ContainsKey(type))
                throw new Exception($"Cannot add item of type {type}. This type already exists");

            _componentsMap[type] = newComponent;

            return newComponent;
        }

        public void Unregister<TP>(TP component) where TP : T
        {
            Type type = component.GetType();
            
            if(!_componentsMap.ContainsKey(type))
                throw new Exception($"item of type {type} doesn't exist in this map");

            _componentsMap.Remove(type);
        }

        public TP Get<TP>() where TP : T
        {
            Type type = typeof(TP);
            
            if(!_componentsMap.ContainsKey(type))
                throw new Exception($"item of type {type} doesn't exist in this map");

            return (TP)_componentsMap[type];
        }
    }
}
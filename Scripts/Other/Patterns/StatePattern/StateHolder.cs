using System;
using PaleLuna.DataHolder;

namespace PaleLuna.Patterns.State
{
    /**
 * @brief Класс StateHolder<T> представляет собой хранилище состояний для паттерна State.
 *
 * StateHolder<T> предоставляет методы для регистрации, удаления и изменения текущего состояния.
 * Он хранит состояния в DictionaryDataHolder<T> и предоставляет удобный способ управления ими.
 *
 * @typeparam T Тип состояний, который должен быть производным от State.
 */
    public class StateHolder<T> : IStateHolder<T> where T : State
    {
        /**
         * @brief Хранилище состояний, реализованное в виде словаря.
         */
        private UniqDataHolder<T> _stateMap;
        /**
        * @brief Текущее состояние.
        */
        public T currentState { get; private set; }

        /**
        * @brief Конструктор StateHolder<T>.
        * Создает новый объект хранилища состояний.
        */
        public StateHolder() => 
            _stateMap = new UniqDataHolder<T>();
        
        /**
         * @brief Регистрация нового состояния.
         *
         * @param item Состояние, которое необходимо зарегистрировать.
         *
         * @return Зарегистрированное состояние.
         */
        public TP Registarion<TP>(TP item) where TP : T => 
            _stateMap.Registration<TP>(item);

        /**
         * @brief Удаление зарегистрированного состояния.
         *
         * @return Удаленное состояние.
         */
        public TP Unregistration<TP>() where TP : T => 
            _stateMap.Unregistration<TP>();

        /**
         * @brief Изменение текущего состояния на новое.
         *
         * @typeparam TP Тип нового состояния.
         */
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
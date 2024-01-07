using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PaleLuna.Attributes;
using PaleLuna.DataHolder;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Initializer;
using UnityEngine;

namespace PaleLuna.Architecture.EntryPoint
{
    /**
     * @brief Абстрактный базовый класс для точек входа приложения.
     *
     * Этот класс предоставляет базовую структуру для управления инициализацией компонентов при запуске приложения.
     */
    public abstract class EntryPoint : MonoBehaviour
    {
        /** @brief Количество элементов по умолчанию для списка инициализаторов. */
        private const int DEFAULT_LIST_CAPACITY = 10;
        
        /** @brief Список объектов, реализующих интерфейс IInitializer. */
        protected List<IInitializer> _initializersList = new(DEFAULT_LIST_CAPACITY);
        
        /**
        * @brief Список объектов, реализующих интерфейс IStartable, предназначенных для автоматического запуска.
        *
        * Объекты в этом списке будут автоматически запускаться после инициализации.
        */
        [Header("Startables")] 
        [SerializeReference, RequireInterface(typeof(IStartable))]
        private List<MonoBehaviour> _startablesMono;
        
        /** @brief Коллекция объектов IStartable для управления запуском. */
        private DataHolder<IStartable> _startables;

        /**
       * @brief Метод для настройки объекта EntryPoint.
       *
       * Этот метод вызывается при инициализации объекта EntryPoint и выполняет загрузку всех сервисов.
       */
        protected virtual async UniTaskVoid Setup() => await LoadAllServices();

        /**
         * @brief Абстрактный метод для заполнения списка инициализаторов.
         *
         * Переопределите этот метод в подклассе, чтобы добавить свои собственные инициализаторы.
         */
        protected virtual void FillInitializers(){}
        
        /**
       * @brief Абстрактный метод для запуска всех инициализаторов.
       *
       * Переопределите этот метод в подклассе, чтобы определить, какие инициализаторы запускать.
       */
        protected virtual void StartAllInitializers(){}
        
        /**
        * @brief Метод для компиляции всех компонентов IStartable из списка _startablesMono.
        *
        * Этот метод создает коллекцию _startables и регистрирует в нее все компоненты IStartable.
        */
        protected void CompileAllComponents()
        {
            _startables = new DataHolder<IStartable>(_startablesMono.Count);
            _startablesMono.ForEach(behaviour => _startables.Registration((IStartable)behaviour));

            _startables.Registration(
                Searcher.ListOfAllByInterface<IStartable>(item => item.IsStarted == false), 
                ListRegistrationType.MergeToEndUnion);
        }
        /**
         * @brief Метод для запуска всех компонентов IStartable.
         *
         * Этот метод вызывает метод OnStart для каждого компонента IStartable в коллекции _startables.
         */
        protected void StartAllComponents() =>
            _startables.ForEach(item => item.OnStart());

        /**
         * @brief Асинхронный метод для загрузки всех сервисов из списка инициализаторов.
         *
         * Этот метод ожидает, пока все инициализаторы не завершат свою работу.
         */
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
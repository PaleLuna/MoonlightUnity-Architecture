using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Initializer;
using PaleLuna.Attributes;
using PaleLuna.DataHolder;
using System.Collections.Generic;
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

        /** @brief Список объектов MonoBehaviour, реализующих интерфейс IInitializer. */
        [SerializeReference, RequireInterface(typeof(IInitializer))]
        private List<MonoBehaviour> _initializersMono = new(DEFAULT_LIST_CAPACITY);

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
        /** @brief Коллекция объектов IInitializer для управления запуском. */
        protected DataHolder<IInitializer> _initializers = new(DEFAULT_LIST_CAPACITY);

        protected virtual void Awake() =>
            _ = Setup();
            
        /**
       * @brief Метод для настройки объекта EntryPoint.
       *
       * Этот метод вызывается при инициализации объекта EntryPoint и выполняет поиск всех компонентов.
       */
        protected virtual async UniTask Setup()
        {
            FillInitializers();
            CompileAllInitializers();
            StartAllInitializers();

            await LoadAllServices();

            CompileAllComponents();
            StartAllComponents();
        }

        /**
         * @brief Метод для заполнения списка инициализаторов.
         *
         * Переопределите этот метод в подклассе, чтобы добавить свои собственные инициализаторы.
         */
        protected virtual void FillInitializers() { }

        /**
       * @brief Метод для запуска всех инициализаторов.
       *
       * Переопределите этот метод в подклассе, чтобы определить, какие инициализаторы запускать.
       */
        protected virtual void StartAllInitializers()
        {
            _initializers.ForEach(initializer =>
            {
                if (initializer.status == InitStatus.Shutdown)
                    initializer.StartInit();
            });
        }

        /**
        * @brief Метод для компиляции всех компонентов IStartable из списка _startablesMono.
        *
        * Этот метод создает коллекцию _startables и регистрирует в нее все компоненты IStartable.
        */
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

        /**
         * @brief Метод для запуска всех компонентов IStartable.
         *
         * Этот метод вызывает метод OnStart для каждого компонента IStartable в коллекции _startables.
         */
        protected void StartAllComponents() => _startables.ForEach(item => item.OnStart());

        /**
         * @brief Асинхронный метод для загрузки всех сервисов из списка инициализаторов.
         *
         * Этот метод ожидает, пока все инициализаторы не завершат свою работу.
         */
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
        }
    }
}

using Cysharp.Threading.Tasks;
using PaleLuna.DataHolder;
using PaleLuna.Patterns.State.Game;
using PaleLuna.Architecture.Controllers;
using PaleLuna.Architecture.Initializer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture.EntryPoint
{
    /**
    * @brief Класс точки входа для запуска игры.
    *
    * Этот класс расширяет базовый класс `EntryPoint` и предоставляет функциональность для
    * настройки и запуска игры, включая инициализацию компонентов, переход между сценами и т.д.
    */
    [AddComponentMenu("Moonlight Unity / Entry Points / Game Boot")]
    public class BootPoint : EntryPoint
    {
        #region Properties

        /** @brief Параметры следующей сцены. */
        [Header("Next scene params")]
        [SerializeField, Min(0)] private int _nextScene = 1;

        /** @brief Объект, который не будет уничтожен при переходе между сценами. */
        private GameObject _dontDestroyObject;

        #endregion

        #region Mono methods

        /**
         * @brief Метод, вызываемый в редакторе Unity при валидации объекта.
         *
         * Ограничивает значение _nextScene от 0 до общего количества сцен в проекте.
         */
        private void OnValidate() => 
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCount);

        /**
        * @brief Метод, вызываемый при старте объекта в сцене.
        *
        * Асинхронно запускает метод Setup для инициализации и запуска игры.
        */
        private void Start() =>
           _ = Setup();
        
        #endregion
        
        /**
        * @brief Асинхронный метод для настройки и запуска игры.
        *
        * Создает объект "DontDestroy" для предотвращения уничтожения при переходе между сценами.
        * Инициализирует ServiceLocator, заполняет и запускает инициализаторы, загружает сервисы,
        * изменяет состояние игры, компилирует и запускает компоненты IStartable, переходит к следующей сцене.
        */
        protected override async UniTaskVoid Setup()
        {
            _dontDestroyObject = new GameObject("DontDestroy");
            DontDestroyOnLoad(_dontDestroyObject);
            
            await UniTask.Yield();

            _ = _dontDestroyObject.AddComponent<ServiceLocator>();

            FillInitializers();
            StartAllInitializers();

            await LoadAllServices();

            await UniTask.Yield();
            
            ServiceLocator.Instance.
                GetComponent<GameController>()
                .stateHolder
                .ChangeState<PlayState>();

            CompileAllComponents();
            StartAllComponents();
            
            JumpToScene();
        }
        
        #region Auxiliary methods

        /**
         * @brief Заполняет список инициализаторов необходимыми объектами.
         *
         * Добавляет GameControllerIInitializer в список инициализаторов.
         */
        protected override void FillInitializers() =>
            _initializersList
                .Add(new GameControllerInitializer(_dontDestroyObject));

        /**
        * @brief Запускает все инициализаторы.
        */
        protected override void StartAllInitializers() => 
            _initializersList.ForEach(initializer => initializer.StartInit());

        /**
         * @brief Метод для перехода к указанной сцене.
         *
         * Если параметр sceneNum не указан (по умолчанию -1), используется значение _nextScene.
         */
        private void JumpToScene(int sceneNum = -1) =>
            SceneManager.LoadScene(
                sceneNum < 0 ? _nextScene : sceneNum);

        #endregion
    }
}
using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Controllers;
using PaleLuna.Architecture.Initializer;
using PaleLuna.Architecture.Services;
using PaleLuna.Patterns.State.Game;
using Services;
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
        [SerializeField, Min(0)]
        private int _nextScene = 1;

        /** @brief Объект, который не будет уничтожен при переходе между сценами. */
        private GameObject _dontDestroyObject;

        private ServiceLocator _globalServiceLocator = new ServiceLocator();
        #endregion

        #region Mono methods

        /**
         * @brief Метод, вызываемый в редакторе Unity при валидации объекта.
         *
         * Ограничивает значение _nextScene от 0 до общего количества сцен в проекте.
         */
        private void OnValidate()
        {
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCountInBuildSettings-1);
        }

        #endregion

        /**
        * @brief Асинхронный метод для настройки и запуска игры.
        *
        * Создает объект "DontDestroy" для предотвращения уничтожения при переходе между сценами.
        * Инициализирует ServiceLocator, заполняет и запускает инициализаторы, загружает сервисы,
        * изменяет состояние игры, компилирует и запускает компоненты IStartable, переходит к следующей сцене.
        */
        protected override async UniTask Setup()
        {
            _dontDestroyObject = new GameObject("DontDestroy");
            DontDestroyOnLoad(_dontDestroyObject);

            _ = _dontDestroyObject.AddComponent<ServiceManager>();

            ServiceManager.Instance.GlobalServices = _globalServiceLocator;
            _globalServiceLocator.Registarion(new SceneLoaderService());


            await base.Setup();

            _globalServiceLocator
                .Get<GameController>()
                .stateHolder.ChangeState<PlayState>();

            JumpToScene();
        }

        #region Auxiliary methods

        /**
         * @brief Заполняет список инициализаторов необходимыми объектами.
         *
         * Добавляет GameControllerIInitializer в список инициализаторов.
         */
        protected override void FillInitializers() =>
            _initializers.Registration(new GameControllerInitializer(_dontDestroyObject));

        /**
         * @brief Метод для перехода к указанной сцене.
         *
         * Если параметр sceneNum не указан (по умолчанию -1), используется значение _nextScene.
         */
        protected void JumpToScene(int sceneNum = -1)
        {
            SceneLoaderService sceneService = _globalServiceLocator.Get<SceneLoaderService>();

            sceneService.LoadScene(sceneNum < 0 ? _nextScene : sceneNum);
        }
        #endregion
    }
}

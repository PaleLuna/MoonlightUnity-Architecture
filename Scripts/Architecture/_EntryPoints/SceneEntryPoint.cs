using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PaleLuna.Architecture.EntryPoint
{
    [AddComponentMenu("Moonlight Unity / Entry Points / Scene Boot")]
    public class SceneEntryPoint : EntryPoint
    {
        private GameObject _sceneLocator;
        protected Scene _sceneServiceLocator;

        /**
        * @brief Асинхронный метод для настройки и запуска игры.
        *
        * Создает объект "DontDestroy" для предотвращения уничтожения при переходе между сценами.
        * Инициализирует ServiceLocator, заполняет и запускает инициализаторы, загружает сервисы,
        * изменяет состояние игры, компилирует и запускает компоненты IStartable, переходит к следующей сцене.
        */
        protected override async UniTask Setup()
        {
            await base.Setup();

            await UniTask.Yield();

            print("SceneBoot");

            _sceneLocator = new GameObject("SceneLocator");

            _sceneServiceLocator = _sceneLocator.AddComponent<Scene>();

            FillSceneLocator();
            ProcessBaggage();
        }

        protected virtual void FillSceneLocator() { }

        protected virtual void ProcessBaggage() { }
    }
}

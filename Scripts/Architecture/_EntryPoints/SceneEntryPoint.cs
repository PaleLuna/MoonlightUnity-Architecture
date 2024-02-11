using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Services;
using Services;
using System;
using System.Threading;
using UnityEngine;

namespace PaleLuna.Architecture.EntryPoint
{
    /**
 * @brief Класс для точек входа сцен.
 *
 * Этот класс предоставляет базовую структуру для управления инициализацией компонентов при запуске сцены.
 */
    [AddComponentMenu("Moonlight Unity / Entry Points / Scene Boot")]
    public class SceneEntryPoint : EntryPoint
    {
        protected ServiceLocator _sceneServiceLocator = new ServiceLocator();

        /**
        * @brief Асинхронный метод для настройки и запуска игры.
        *
        * Создает объект "DontDestroy" для предотвращения уничтожения при переходе между сценами.
        * Инициализирует ServiceLocator, заполняет и запускает инициализаторы, загружает сервисы,
        * изменяет состояние игры, компилирует и запускает компоненты IStartable, переходит к следующей сцене.
        */
        protected override async UniTask Setup()
        {
            if(ServiceManager.Instance == null)
            {
                SceneLoaderService sceneLoader = new();

                sceneLoader.LoadScene(0);

                throw new OperationCanceledException("ServiceManager is null. Reload");
            }
            ServiceManager.Instance.SceneLocator = _sceneServiceLocator;
            FillSceneLocator();

            await UniTask.Yield();

            await base.Setup();

            ProcessBaggage();
        }

        protected virtual void FillSceneLocator() { }

        protected virtual void ProcessBaggage() { }
    }
}

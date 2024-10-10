using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Services;
using Services;
using System;
using System.Threading;
using UnityEngine;

namespace PaleLuna.Architecture.EntryPoint
{

    [AddComponentMenu("Moonlight Unity / Entry Points / Scene Boot")]
    public class SceneEntryPoint : EntryPoint
    {
        protected ServiceLocator _sceneServiceLocator = new ServiceLocator();

        protected override async UniTask Setup()
        {
            if(ServiceManager.Instance == null)
            {
                SceneLoaderService sceneLoader = new();

                sceneLoader.LoadScene(0);

                throw new OperationCanceledException("ServiceManager is null. Reload");
            }
            ServiceManager.Instance.LocalServices = _sceneServiceLocator;
            FillSceneLocator();

            await UniTask.Yield();

            await base.Setup();

            ProcessBaggage();
        }

        protected virtual void FillSceneLocator() { }

        protected virtual void ProcessBaggage() { }
    }
}

using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.EntryPoint;
using System.Collections;
using UnityEngine;

namespace Assets.MoonlightUnity_Architecture.Scripts.Architecture._EntryPoints
{
    public class TestPoint : SceneEntryPoint
    {
        protected override async UniTask Setup()
        {
            await base.Setup();

            print("Hello, Scene!");
        }
    }
}
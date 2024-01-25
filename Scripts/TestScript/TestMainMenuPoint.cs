using System;
using PaleLuna.Architecture.EntryPoint;
using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;
using Services;

namespace MoonlightUnity_Architecture.Scripts.TestScript
{
    public class TestMainMenuPoint : EntryPoint
    {
        protected override void Start()
        {
            _ = Setup();

            print("Test point");
            SceneBaggage sceneBaggage = ServiceManager
                .Instance
                .GlobalServices.Get<SceneLoaderService>().GetBaggage();

            if (sceneBaggage == null) return;

            print(sceneBaggage.GetFloat("MyFloat"));
            print(sceneBaggage.GetInt("MyInt"));
            print(sceneBaggage.GetBool("MyBool"));
        }

        protected override void FillInitializers(){ }
    }
}
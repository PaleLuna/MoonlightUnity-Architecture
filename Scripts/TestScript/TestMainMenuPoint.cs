using System;
using PaleLuna.Architecture.EntryPoint;
using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;

namespace MoonlightUnity_Architecture.Scripts.TestScript
{
    public class TestMainMenuPoint : EntryPoint
    {
        private void Start()
        {
            SceneBaggage sceneBaggage = ServiceLocator.Instance.Get<SceneService>().GetBaggage();
            
            
            print(sceneBaggage.GetFloat("MyFloat"));
            print(sceneBaggage.GetInt("MyInt"));
            print(sceneBaggage.GetBool("MyBool"));
            
            print(sceneBaggage.GetInt("MyFloat"));

        }
    }
}
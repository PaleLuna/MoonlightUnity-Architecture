using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture.Services
{
    public class SceneLoaderService : IService
    {
        private SceneBaggage _sceneBaggage = null;
        
        public void LoadScene(int sceneNum)
        {
            SceneManager.LoadScene(sceneNum);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public SceneLoaderService SetBaggage(SceneBaggage sceneBaggage)
        {
            _sceneBaggage = sceneBaggage;
            return this;
        }

        public SceneBaggage GetBaggage()
        {
            return _sceneBaggage;
        }
    }
}
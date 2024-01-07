using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture.Services
{
    public class SceneService : IService
    {
        private SceneBaggage _sceneBaggage;
        
        public void LoadScene(int sceneNum)
        {
            SceneManager.LoadScene(sceneNum);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public SceneService SetBaggage(SceneBaggage sceneBaggage)
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
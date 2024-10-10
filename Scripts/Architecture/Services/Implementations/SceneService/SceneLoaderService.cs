using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture.Services
{
    public class SceneLoaderService : IService
    {
        private SceneBaggage _nextSceneBaggage = null;
        private SceneBaggage _currentSceneBaggage = null;

        private AsyncOperation _asyncOperation;

        public bool sceneIsLoad
        {
            get
            {
                if(_asyncOperation == null)
                    return false;
                return _asyncOperation.isDone;
            }
        }
        
        public void LoadScene(int sceneNum)
        {
            SceneManager.LoadScene(sceneNum);
        }

        public void LoadScene(string sceneName)
        {
            SwapBaggage();
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(int sceneNum, bool allowSceneActivation = true)
        {
            SwapBaggage();
            _asyncOperation = SceneManager.LoadSceneAsync(sceneNum);

            _asyncOperation.allowSceneActivation = allowSceneActivation;
        }
        public void AllowNextScene()
        {
            if (_asyncOperation != null) _asyncOperation.allowSceneActivation = true;

            _asyncOperation = null;
        }

        public SceneLoaderService SetNextBaggage(SceneBaggage sceneBaggage)
        {
            _nextSceneBaggage = sceneBaggage;
            return this;
        }

        public SceneBaggage GetNextBaggage()
        {
            if (_nextSceneBaggage == null)
                _nextSceneBaggage = new();

            return _nextSceneBaggage;
        }

        public SceneBaggage GetCurrentBaggage()
        {
            if (_currentSceneBaggage == null)
                throw new NullReferenceException("current scene baggage is null");

            return _currentSceneBaggage;
        }

        private void SwapBaggage()
        {
            _currentSceneBaggage = _nextSceneBaggage;
            _nextSceneBaggage = null;
        }

    }
}
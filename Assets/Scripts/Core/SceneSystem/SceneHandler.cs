using System;
using System.Collections;
using Tzipory.Tools.Debag;
using Tzipory.Tools.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tzipory.Systems.SceneSystem
{
    public class SceneHandler : MonoBehaviour , ISceneHandler
    {
        private const string  SCENE_HANDLER_LOG__GROUP = "SceneHandler";
        
        public static event Action<SceneType> OnSceneLoaded;
        
        private const int MAIN_MENU_SCENE_INDEX = 1;
        private const int MAP_SCENE_INDEX = 2;
        private const int GAME_SCENE_INDEX = 3;
        
        [SerializeField] private float _minLoadTime;
        
        [SerializeField] private LoadingScreenHandler _loadingScreenHandler;
        
        private float _loadTime;
        
        public static Scene PresistanteScene { get; private set; }
        public static Scene CurrentScene { get; private set; }
        
        public static bool IsLoading { get; private set; }

        private void Start()
        {
            PresistanteScene = SceneManager.GetActiveScene();

#if UNITY_EDITOR
            _minLoadTime = 0;
#endif
        }

        private IEnumerator LoadSceneAsync(SceneType sceneType)
        {
            IsLoading = true;
            _loadTime = 0;
            
            int sceneIndex = sceneType switch
            {
                SceneType.MainMenu => MAIN_MENU_SCENE_INDEX,
                SceneType.Map => MAP_SCENE_INDEX,
                SceneType.Game => GAME_SCENE_INDEX,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
            };

            Scene preventsScene = SceneManager.GetActiveScene();

            if (preventsScene.buildIndex != 0)
            {
                SceneManager.SetActiveScene(PresistanteScene);
                CoreLogger.Log($"Unloading scene {preventsScene.name}",SCENE_HANDLER_LOG__GROUP);

                yield return _loadingScreenHandler.FadeIn();
                
                var unloadSceneAsync = SceneManager.UnloadSceneAsync(preventsScene);
                
                yield return SceneLoaderAndUnLoader(unloadSceneAsync);
                
                CoreLogger.Log($"Unloaded scene {preventsScene.name}",SCENE_HANDLER_LOG__GROUP);
            }

            CoreLogger.Log($"start loading sceneType",SCENE_HANDLER_LOG__GROUP);
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Additive);
            
            loadSceneAsync.allowSceneActivation = false;

            yield return SceneLoaderAndUnLoader(loadSceneAsync);

            if (_loadTime < _minLoadTime)
            {
                var wait = new WaitForSeconds(_minLoadTime - _loadTime);
                yield return wait;
            }
            
            CurrentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            
            yield return  _loadingScreenHandler.FadeOut();
            
            SceneManager.SetActiveScene(CurrentScene);
            
            OnSceneLoaded?.Invoke(sceneType);

            IsLoading = false;
            
            CoreLogger.Log($"Loaded sceneType {CurrentScene.name}",SCENE_HANDLER_LOG__GROUP);
        }

        private IEnumerator SceneLoaderAndUnLoader(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                _loadTime += Time.deltaTime;
                
                if (asyncOperation.progress  >= 0.9f)
                    asyncOperation.allowSceneActivation = true;
                
                CoreLogger.Log($"loading progress {asyncOperation.progress * 100}%",SCENE_HANDLER_LOG__GROUP);
                yield return null;
            }
        }

        public void LoadScene(SceneType sceneType)
        {
            StartCoroutine(LoadSceneAsync(sceneType));
        }

        private void OnValidate()
        {
            if (_loadingScreenHandler == null)
                _loadingScreenHandler = GetComponent<LoadingScreenHandler>();
        }
    }

    public interface ISceneHandler
    {
        public void LoadScene(SceneType sceneType);
    }

    public enum SceneType
    {
        MainMenu,
        Map,
        Game
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string gameScene;
    [SerializeField] string gameUIScene;

    [SerializeField] GameObject mainMenuRoot;
    [SerializeField] Button startGameButton;

    private void Awake()
    {
        GameUI.RestartButtonClickedEvent += RestartButtonClicked_EventHandler;
        startGameButton.onClick.AddListener(StartButtonClicked_EventHandler);
    }

    private void StartButtonClicked_EventHandler()
    {
        mainMenuRoot.SetActive(false);
        var op = SceneManager.LoadSceneAsync(gameUIScene, LoadSceneMode.Additive);
        op.completed += GameUISceneLoadedComlete_EventHandler;
    }

    // Loading Game scene after GameUI scene to make sure GameUI scene properly handles first CurrentPlayerChangedEvent event 
    private void GameUISceneLoadedComlete_EventHandler(AsyncOperation obj)
    {
        SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
    }

    private void RestartButtonClicked_EventHandler()
    {
        SceneManager.UnloadSceneAsync(gameScene);
        SceneManager.UnloadSceneAsync(gameUIScene);
        mainMenuRoot.SetActive(true);
    }

    private void OnDestroy()
    {
        GameUI.RestartButtonClickedEvent -= RestartButtonClicked_EventHandler;
    }
}

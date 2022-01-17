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
        SceneManager.LoadScene(gameScene, LoadSceneMode.Additive);
        SceneManager.LoadScene(gameUIScene, LoadSceneMode.Additive);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentPlayerLabel;
    [SerializeField] GameObject _victoryLabelWrapper;
    [SerializeField] TextMeshProUGUI _victoryLabel;
    [SerializeField] PlayerTurnCountUI _turnCountUI_Player1;
    [SerializeField] PlayerTurnCountUI _turnCountUI_Player2;
    [SerializeField] Button _restartButton;

    PlayerTurnCountUI[] _turnCountUIs;

    public static event Action RestartButtonClickedEvent;


    void Awake()
    {
        _turnCountUIs = new PlayerTurnCountUI[] { _turnCountUI_Player1, _turnCountUI_Player2 };
        _victoryLabelWrapper.SetActive(false);

        _restartButton.onClick.AddListener(() =>
        {
            _victoryLabelWrapper.SetActive(false);
            gameObject.SetActive(false);
            RestartButtonClickedEvent?.Invoke();
        });

        GameController.CurrentPlayerChangedEvent += CurrentPlayerChanged_EventHandler;
        GameController.WinnerDeterminedEvent += WinnerDetermined_EventHandler;
    }


    private void WinnerDetermined_EventHandler(Player player)
    {
        _victoryLabelWrapper.SetActive(true);
        _victoryLabel.text = $"{player.Name} won!";
    }


    private void CurrentPlayerChanged_EventHandler((List<Player> players, int activePlayerIndex) info)
    {
        for (int i = 0; i < info.players.Count; i++)
        {
            _turnCountUIs[i].Refresh(info.players[i]);
        }
        _currentPlayerLabel.text = $"{info.players[info.activePlayerIndex].Name}";
    }


    private void OnDestroy()
    {
        GameController.CurrentPlayerChangedEvent -= CurrentPlayerChanged_EventHandler;
        GameController.WinnerDeterminedEvent -= WinnerDetermined_EventHandler;
    }
}

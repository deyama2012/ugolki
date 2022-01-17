using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentPlayerLabel;
    [SerializeField] TextMeshProUGUI _victoryLabel;

    [SerializeField] PlayerTurnCountUI _turnCountUI_Player1;
    [SerializeField] PlayerTurnCountUI _turnCountUI_Player2;


    PlayerTurnCountUI[] _turnCountUIs;


    void Awake()
    {
        _turnCountUIs = new PlayerTurnCountUI[] { _turnCountUI_Player1, _turnCountUI_Player2 };

        _victoryLabel.gameObject.SetActive(false);

        GameController.CurrentPlayerChangedEvent += CurrentPlayerChanged_EventHandler;
        GameController.WinnerDeterminedEvent += WinnerDetermined_EventHandler;
    }


    private void WinnerDetermined_EventHandler(Player player)
    {
        _victoryLabel.gameObject.SetActive(true);
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

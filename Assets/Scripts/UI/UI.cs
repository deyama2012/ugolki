using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentPlayerLabel;

    [SerializeField] PlayerTurnCountUI _turnCountUI_Player1;
    [SerializeField] PlayerTurnCountUI _turnCountUI_Player2;

    PlayerTurnCountUI[] _turnCountUIs;

    void Awake()
    {
        _turnCountUIs = new PlayerTurnCountUI[] { _turnCountUI_Player1, _turnCountUI_Player2 };

        GameController.CurrentPlayerChangedEvent += GameController_CurrentPlayerChangedEvent;
    }

    private void GameController_CurrentPlayerChangedEvent((List<Player> players, int activePlayerIndex) info)
    {
        for (int i = 0; i < info.players.Count; i++)
        {
            _turnCountUIs[i].Refresh(info.players[i]);
        }
        _currentPlayerLabel.text = $"{info.players[info.activePlayerIndex].Name}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerTurnCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _turnCountCaption;
    [SerializeField] TextMeshProUGUI _turnCountValue;

    public void Refresh(Player player)
    {
        if (_turnCountCaption.text != player.Name)
            _turnCountCaption.text = $"{player.Name}:";

        _turnCountValue.text = $"{player.TurnCount}";
    }
}

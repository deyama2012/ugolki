using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] Player _player1;
    [SerializeField] Player _player2;

    int _activePlayerIndex;
    List<Player> _players;

    public Player CurrentPlayer => _players[_activePlayerIndex];

    public static event Action<(List<Player> players, int activePlayerIndex)> CurrentPlayerChangedEvent;


    private void Start()
    {
        _player1 = new Player("White Player");
        _player2 = new Player("Black Player");
        _players = new List<Player>() { _player1, _player2 };

        _board.CreateBoard();
        _board.CreatePieces(_player1, _player2);
        _board.SetDependency(this);

        CurrentPlayerChangedEvent?.Invoke((_players, _activePlayerIndex));
    }


    public void EndOfTurn()
    {
        _players[_activePlayerIndex].IncrementTurnCount();
        _activePlayerIndex = (_activePlayerIndex + 1) % _players.Count;
        CurrentPlayerChangedEvent?.Invoke((_players, _activePlayerIndex));
    }
}

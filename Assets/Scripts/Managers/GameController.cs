using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] Player _playerPrefab;
    [SerializeField] Player _player1;
    [SerializeField] Player _player2;

    int _activePlayerIndex;
    List<Player> _players;

    public bool InputAllowed { get; private set; }

    public Player CurrentPlayer => _players[_activePlayerIndex];

    public static event Action<(List<Player> players, int activePlayerIndex)> CurrentPlayerChangedEvent;
    public static event Action<Player> WinnerDeterminedEvent;


    private void Start()
    {
        _player1 = Instantiate(_playerPrefab, transform);
        _player1.SetName("White Player");

        _player2 = Instantiate(_playerPrefab, transform);
        _player2.SetName("Black Player");

        _players = new List<Player>() { _player1, _player2 };

        _board.CreateBoard();
        _board.CreateCamps(_player1, _player2);
        _board.CreatePieces(_player1, _player2);
        _board.SetDependency(this);
        _board.DetermineMovementRules();

        InputAllowed = true;

        CurrentPlayerChangedEvent?.Invoke((_players, _activePlayerIndex));
    }


    public void OnEndOfTurn()
    {
        CurrentPlayer.IncrementTurnCount();

        int otherPlayerIndex = (_activePlayerIndex + 1) % _players.Count;
        Player otherPlayer = _players[otherPlayerIndex];

        if (otherPlayer.Camp.IsCaptured())
        {
            InputAllowed = false;
            WinnerDeterminedEvent?.Invoke(CurrentPlayer);
        }
        else
        {
            _activePlayerIndex = otherPlayerIndex;
            CurrentPlayerChangedEvent?.Invoke((_players, _activePlayerIndex));
        }
    }
}

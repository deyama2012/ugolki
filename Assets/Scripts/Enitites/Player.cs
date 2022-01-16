using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    [SerializeField] string _name;
    [SerializeField] int turnCount;
    [SerializeField] List<Piece> _pieces;

    public string Name => _name;
    public int TurnCount => turnCount;

    public IEnumerable<Piece> Pieces
    {
        get
        {
            foreach (var piece in _pieces)
                yield return piece;
        }
    }

    //public static event Action<(Player player, int turnCount)> TurnCountChangedEvent;

    public Player(string name) => _name = name;

    public void IncrementTurnCount()
    {
        turnCount++;
        //TurnCountChangedEvent?.Invoke((this, turnCount));
    }

    public void AssignPieces(List<Piece> pieces)
    {
        _pieces = pieces;
        _pieces.ForEach(p => p.SetOwner(this));
    }
}

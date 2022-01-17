using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Camp
{
    [SerializeField] Player _owner;
    [SerializeField] List<Square> _squares;

    public Player Owner => _owner;

    public IEnumerable<Square> Squares
    {
        get
        {
            foreach (var square in _squares)
                yield return square;
        }
    }

    public Camp(List<Square> squares, Player owner)
    {
        _squares = squares;
        _owner = owner;
    }

    public bool IsCaptured()
    {
        int numCapturedSquares = 0;
        foreach (var square in _squares)
        {
            if (square.OccupiedBy != null && square.OccupiedBy.Owner != _owner)
                numCapturedSquares++;
        }
        bool captured = numCapturedSquares == _squares.Count;
        return captured;
    }
}

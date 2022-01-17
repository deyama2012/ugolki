using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MoveInfo
{
    /// <summary>
    /// Is used to be able to rebuild path during piece movement animation later.
    /// For every available square address (key) stores another address that led to this square (value) 
    /// </summary>
    Dictionary<Vector2Int, Vector2Int?> _visitedFrom = new Dictionary<Vector2Int, Vector2Int?>();

    public Piece piece;

    /// <summary>Destination.</summary>
    public Square square;

    public ICollection<Vector2Int> availableDestinations => _visitedFrom.Keys;


    public void GetAvailableMoves(Square[,] grid, MovementRule movementRule)
    {
        movementRule.GetAvailableMoves(this, grid);
    }


    public void AddAvailableMove(Vector2Int square, Vector2Int? precedingSquare)
    {
        _visitedFrom[square] = precedingSquare;
    }


    public bool IsVisited(Vector2Int square)
    {
        return _visitedFrom.ContainsKey(square);
    }


    public void ExecuteMove(Action onComplete)
    {
        var waypoints = RebuildPath();
        piece.Move(waypoints, onComplete);
    }


    /// <summary>Rebuilds path to the destination node via backtracking</summary>
    private List<Vector2Int> RebuildPath()
    {
        Vector2Int current = square.Address;
        var path = new List<Vector2Int>();
        path.Add(current);
        while (current != null && current != piece.Address)
        {
            current = _visitedFrom[current].Value;
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}

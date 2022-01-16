using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementRule : ScriptableObject
{
    protected Vector2Int[] _moveDirections =
    {
        Vector2Int.up,
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left,
        Vector2Int.left + Vector2Int.up
    };

    public abstract List<Vector2Int> GetAvailableMoves(Piece piece, Piece[,] grid);

    protected bool IsOutOfBounds(Vector2Int address, Piece[,] grid) => address.y < 0 || address.y >= grid.GetLength(0) || address.x < 0 || address.x >= grid.GetLength(1);
    protected bool IsOccupied(Vector2Int address, Piece[,] grid) => grid[address.y, address.x] != null;
}

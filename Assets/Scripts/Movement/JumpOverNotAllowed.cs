using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Rules/Jump over not allowed")]
public class JumpOverNotAllowed : MovementRule
{
    public override List<Vector2Int> GetAvailableMoves(Piece piece, Piece[,] grid)
    {
        var availableDestinations = new List<Vector2Int>();
        Vector2Int origin = piece.Address;
        foreach (var dir in _moveDirections)
        {
            var destination = origin + dir;
            if (IsOutOfBounds(destination, grid) || IsOccupied(destination, grid))
                continue;
            availableDestinations.Add(destination);
        }
        return availableDestinations;
    }
}

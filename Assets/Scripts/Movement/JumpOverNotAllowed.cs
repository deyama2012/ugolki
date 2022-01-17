using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Rules/Jumpover not allowed")]
public class JumpOverNotAllowed : MovementRule
{
    public override void GetAvailableMoves(MoveInfo moveInfo, Square[,] grid)
    {
        Dictionary<Vector2Int, Vector2Int?> visitedFrom = new Dictionary<Vector2Int, Vector2Int?>();

        Vector2Int origin = moveInfo.piece.Address;
        foreach (var dir in _moveDirections)
        {
            var destination = origin + dir;

            if (IsOutOfBounds(destination, grid) || IsOccupied(destination, grid))
                continue;

            // Mark as visited (= available) and also store address that led to this address
            moveInfo.AddAvailableMove(destination, origin);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Rules/Jumpover allowed")]
public class JumpOverAllowed : MovementRule
{
    HashSet<Vector2Int> _jumpDirectionsStraight = new HashSet<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    HashSet<Vector2Int> _jumpDirectionsDiagonal = new HashSet<Vector2Int> { Vector2Int.up + Vector2Int.right, Vector2Int.right + Vector2Int.down, Vector2Int.down + Vector2Int.left, Vector2Int.left + Vector2Int.up };

    public enum JumpDirections { Straight, Diagonal }

    [SerializeField] JumpDirections _jumpDirections;


    public override void GetAvailableMoves(MoveInfo moveInfo, Square[,] grid)
    {
        Vector2Int origin = moveInfo.piece.Address;
        foreach (var dir in _moveDirections)
        {
            var destination = origin + dir;
            if (IsOutOfBounds(destination, grid) || IsOccupied(destination, grid))
                continue;

            // Mark as visited (= available) and also store address that led to this address
            moveInfo.AddAvailableMove(destination, origin);
        }

        GetMovesAvailableViaJumps(moveInfo, grid);
    }


    /// <summary>Breadth-first search for squares reachable by jumping over pieces</summary>
    void GetMovesAvailableViaJumps(MoveInfo moveInfo, Square[,] grid)
    {
        HashSet<Vector2Int> jumpDirections = _jumpDirections == JumpDirections.Diagonal ? _jumpDirectionsDiagonal : _jumpDirectionsStraight;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int origin = moveInfo.piece.Address;
        queue.Enqueue(origin);
        moveInfo.AddAvailableMove(origin, null);

        while (queue.Count > 0)
        {
            var square = queue.Dequeue();

            if (IsOutOfBounds(square, grid))
                continue;

            foreach (var dir in jumpDirections)
            {
                var neighbourClose = square + dir;

                // Not interested if out of bounds or DOESN'T have a piece (= no piece to jump over)
                if (IsOutOfBounds(neighbourClose, grid) || !IsOccupied(neighbourClose, grid))
                    continue;

                var neighbourFar = neighbourClose + dir;

                // Not interested if out of bounds or DOES have a piece (= we can't jump to this square) or already visited
                if (IsOutOfBounds(neighbourFar, grid) || IsOccupied(neighbourFar, grid) || moveInfo.IsVisited(neighbourFar))
                    continue;

                queue.Enqueue(neighbourFar);

                // Mark as visited (= available) and also store address that led to this address
                moveInfo.AddAvailableMove(neighbourFar, square);
            }
        }
    }
}

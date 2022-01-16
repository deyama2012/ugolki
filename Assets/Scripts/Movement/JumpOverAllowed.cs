using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Rules/Jump over allowed")]
public class JumpOverAllowed : MovementRule
{
    public enum JumpDirections { Straight, Diagonal }

    [SerializeField] JumpDirections jumpDirections;

    HashSet<Vector2Int> _jumpDirectionsDiagonal = new HashSet<Vector2Int>
    {
        Vector2Int.up + Vector2Int.right,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.down + Vector2Int.left,
        Vector2Int.left + Vector2Int.up
    };

    HashSet<Vector2Int> _jumpDirectionsStraight = new HashSet<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

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

        var availableViaJumps = AvailableViaJumps(piece, grid);
        foreach (var address in availableViaJumps)
        {
            if (!availableDestinations.Contains(address))
            {
                availableDestinations.Add(address);
            }
        }
        return availableDestinations;
    }

    private IEnumerable<Vector2Int> AvailableViaJumps(Piece piece, Piece[,] grid)
    {
        HashSet<Vector2Int> jumDirections = jumpDirections == JumpDirections.Diagonal ? _jumpDirectionsDiagonal : _jumpDirectionsStraight;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        Vector2Int origin = piece.Address;

        visited.Add(origin);
        queue.Enqueue(origin);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (IsOutOfBounds(node, grid))
                continue;

            foreach (var dir in jumDirections)
            {
                var neighbourClose = node + dir;

                // Not interested if out of bounds or DOESN'T have a piece (= no piece to jump over)
                if (IsOutOfBounds(neighbourClose, grid) || !IsOccupied(neighbourClose, grid))
                    continue;

                var neighbourFar = neighbourClose + dir;

                // Not interested if out of bounds or DOES have a piece (= we can't jump to this square) or already visited
                if (IsOutOfBounds(neighbourFar, grid) || IsOccupied(neighbourFar, grid) || visited.Contains(neighbourFar))
                    continue;

                visited.Add(neighbourFar);
                queue.Enqueue(neighbourFar);
            }
        }

        return visited;
    }
}

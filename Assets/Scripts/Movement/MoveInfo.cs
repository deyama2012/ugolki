using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveInfo
{
    public Piece piece;
    public Square square;
    public List<Vector2Int> availableDestinations;
}

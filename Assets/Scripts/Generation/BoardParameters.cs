using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardParameters : ScriptableObject
{
    [Range(2, 26)]
    [SerializeField] private int _squaresPerSide;
    [SerializeField] private float _squareSize;

    [Min(1)]
    [SerializeField] private int _pieceCountPerPlayer;

    public int SquaresPerSide => _squaresPerSide;
    public float SquareSize => _squareSize;
    public int PieceCountPerPlayer => _pieceCountPerPlayer;

    public Vector3 TotalSize => new Vector3(1, 0, 1) * _squaresPerSide * _squareSize;
    public Vector3 BottomLeftSquareCenter => (-TotalSize + new Vector3(1, 0, 1) * _squareSize) * 0.5f;


    private void OnValidate()
    {
        _pieceCountPerPlayer = Mathf.Min(_pieceCountPerPlayer, _squaresPerSide / 2);
    }


    public Vector2Int AddressFromWorldPosition(Vector3 worldPos)
    {
        var offsetFromBottomLeftSquare = worldPos - BottomLeftSquareCenter;
        int x = Mathf.RoundToInt(offsetFromBottomLeftSquare.x / _squareSize);
        int y = Mathf.RoundToInt(offsetFromBottomLeftSquare.z / _squareSize);
        return new Vector2Int(x, y);
    }

    public Vector3 AddressToWorldPosition(Vector2Int address) =>
        BottomLeftSquareCenter + new Vector3(address.x, 0, address.y) * _squareSize;



    //public string AddressToString(Vector2Int address)
    //{
    //    return $"{BoardGenerator.letters[address.x]}{BoardGenerator.numbers[address.y]}";
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardParameters : ScriptableObject
{
    [Range(2, 26)]
    [SerializeField] private int _squaresPerBoardSide = 8;
    [SerializeField] private float _squareSize = 1;

    [Tooltip("Number of player pieces on one axis (meaning that actual number of player pieces will be equal to power of two of that number)")]
    [Min(1)]
    [SerializeField] private int _piecesPerPlayerCampSide = 3;

    public int SquaresPerSide => _squaresPerBoardSide;
    public float SquareSize => _squareSize;
    public int PiecesPerPlayerCampSide => _piecesPerPlayerCampSide;

    public Vector3 TotalSize => new Vector3(1, 0, 1) * _squaresPerBoardSide * _squareSize;

    Vector3 BottomLeftSquareCenter => (-TotalSize + new Vector3(1, 0, 1) * _squareSize) * 0.5f;


    private void OnValidate()
    {
        _piecesPerPlayerCampSide = Mathf.Min(_piecesPerPlayerCampSide, _squaresPerBoardSide / 2);
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
}

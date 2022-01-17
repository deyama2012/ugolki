using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CampsCreator : MonoBehaviour
{
    BoardParameters _boardParams;

    [SerializeField] Camp _campPrefab;

    public void CreateCamps(Square[,] grid, Player player1, Player player2, BoardParameters boardParams)
    {
        _boardParams = boardParams;

        // Start at the bottom left corner and increment x and y indices
        Vector2Int bottomLeftSquareAddress = Vector2Int.zero;
        var squares1 = GetCampSquares(grid, bottomLeftSquareAddress, Vector2Int.one);
        var camp1 = Instantiate(_campPrefab, player1.transform);
        camp1.Init(squares1, player1);
        player1.AssignCamp(camp1);

        // Start at the top right corner and decrement x and y indices
        Vector2Int topRightSquareAddress = Vector2Int.one * (_boardParams.SquaresPerSide - 1);
        var squares2 = GetCampSquares(grid, topRightSquareAddress, -Vector2Int.one);
        var camp2 = Instantiate(_campPrefab, player2.transform);
        camp2.Init(squares2, player2);
        player2.AssignCamp(camp2);
    }

    private List<Square> GetCampSquares(Square[,] grid, Vector2Int startAddress, Vector2Int moveDir)
    {
        var squares = new List<Square>();
        int dimension = _boardParams.PiecesPerPlayerCampSide;
        for (int y = 0; y < dimension; y++)
        {
            for (int x = 0; x < dimension; x++)
            {
                var address = startAddress + new Vector2Int(moveDir.x * x, moveDir.y * y);
                squares.Add(grid.Get(address));
            }
        }
        return squares;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    GameController _gameController;
    Piece[,] _grid;

    [SerializeField] BoardParameters _boardParameters;
    [SerializeField] BoardGenerator _boardGenerator;
    [SerializeField] PiecesCreator _pieceManager;
    [SerializeField] MoveInfo _moveInfo;

    public static event Action<List<Vector3>> HighlightRefreshRequestEvent;


    public void CreateBoard()
    {
        _grid = new Piece[_boardParameters.SquaresPerSide, _boardParameters.SquaresPerSide];
        _boardGenerator.GenerateBoard(_boardParameters);
    }


    public void CreatePieces(Player player1, Player player2)
    {
        var pieces = _pieceManager.CreatePlayersPieces(_boardParameters);

        pieces.player1.ForEach(p => _grid[p.Address.y, p.Address.x] = p);
        pieces.player2.ForEach(p => _grid[p.Address.y, p.Address.x] = p);

        player1.AssignPieces(pieces.player1);
        player2.AssignPieces(pieces.player2);
    }


    public void SetDependency(GameController gameController) => _gameController = gameController;


    public void OnPieceSelectionChanged(Piece piece)
    {
        if (piece != null)
        {
            if (piece.Owner == _gameController.CurrentPlayer)
            {
                _moveInfo = new MoveInfo();
                _moveInfo.piece = piece;
                _moveInfo.availableDestinations = GetAvailableDestinations(piece);
            }
            else return;
        }
        RequestHighlightRefresh(_moveInfo);
    }


    public void OnSquareSelectionChanged(Square square)
    {
        if (_moveInfo != null && _moveInfo.piece != null)
        {
            if (_moveInfo.availableDestinations.Contains(square.Address))
            {
                _moveInfo.square = square;
                ExecuteMove(ref _moveInfo);
            }
        }
    }


    private List<Vector2Int> GetAvailableDestinations(Piece piece)
    {
        var availableDestinations = new List<Vector2Int>();
        Vector2Int origin = piece.Address;
        Vector2Int[] moveDirections = { Vector2Int.up, Vector2Int.up + Vector2Int.right, Vector2Int.right, Vector2Int.right + Vector2Int.down, Vector2Int.down, Vector2Int.down + Vector2Int.left, Vector2Int.left, Vector2Int.left + Vector2Int.up };
        foreach (var dir in moveDirections)
        {
            var destination = origin + dir;
            if (IsOutOfBounds(destination) || IsOccupied(destination))
                continue;
            availableDestinations.Add(destination);
        }
        return availableDestinations;

        bool IsOutOfBounds(Vector2Int address) => address.y < 0 || address.y >= _grid.GetLength(0) || address.x < 0 || address.x >= _grid.GetLength(1);
        bool IsOccupied(Vector2Int address) => _grid[address.y, address.x] != null;
    }


    private void ExecuteMove(ref MoveInfo moveInfo)
    {
        _grid[moveInfo.piece.Address.y, moveInfo.piece.Address.x] = null;
        _grid[moveInfo.square.Address.y, moveInfo.square.Address.x] = moveInfo.piece;

        moveInfo.piece.Move(moveInfo.square, _gameController.EndOfTurn);
        moveInfo = null;

        RequestHighlightRefresh(null);
    }


    /// <summary>Pass 'null' to disable highlight</summary>
    void RequestHighlightRefresh(MoveInfo moveInfo)
    {
        var positions = new List<Vector3>();
        if (moveInfo != null)
        {
            positions.Add(AddressToWorldPosition(moveInfo.piece.Address));

            if (moveInfo.availableDestinations != null)
            {
                moveInfo.availableDestinations.ForEach(address => positions.Add(AddressToWorldPosition(address)));
            }
        }
        HighlightRefreshRequestEvent?.Invoke(positions);
    }


    Vector3 AddressToWorldPosition(Vector2Int address) =>
        _boardParameters.BottomLeftSquareCenter + new Vector3(address.x, 0, address.y) * _boardParameters.SquareSize;


    public void DebugLogGrid()
    {
        string grid = string.Empty;
        for (int y = _grid.GetLength(0) - 1; y >= 0; y--)
        {
            string row = string.Empty;
            for (int x = 0; x < _grid.GetLength(1); x++)
            {
                row += _grid[y, x] != null ? "O " : " _ ";
            }
            grid += row + "\r\n";
        }
        Debug.Log(grid);
    }
}

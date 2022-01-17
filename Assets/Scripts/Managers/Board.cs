using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    GameController _gameController;
    Square[,] _grid;

    [SerializeField] BoardParameters _boardParameters;
    [SerializeField] MovementRule movementRule;
    [SerializeField] BoardCreator _boardCreator;
    [SerializeField] CampsCreator _campsCreator;
    [SerializeField] PiecesCreator _piecesCreator;
    [SerializeField] MoveInfo _moveInfo;

    public static event Action<List<Vector3>> HighlightRefreshRequestEvent;


    public void CreateBoard()
    {
        _grid = new Square[_boardParameters.SquaresPerSide, _boardParameters.SquaresPerSide];
        _boardCreator.GenerateBoard(_grid, _boardParameters);
    }


    public void CreateCamps(Player player1, Player player2)
    {
        _campsCreator.CreateCamps(_grid, player1, player2, _boardParameters);
    }


    public void CreatePieces(Player player1, Player player2)
    {
        var pieces = _piecesCreator.PopulateCamps(player1.Camp, player2.Camp, _boardParameters);
        pieces.player1.ForEach(piece => _grid.Get(piece.Address).SetPiece(piece));
        pieces.player2.ForEach(piece => _grid.Get(piece.Address).SetPiece(piece));
    }


    public void SetDependency(GameController gameController) => _gameController = gameController;


    public void OnPieceSelectionChanged(Piece piece)
    {
        if (!_gameController.InputAllowed) return;
        if (piece != null)
        {
            if (piece.Owner == _gameController.CurrentPlayer)
            {
                _moveInfo = new MoveInfo();
                _moveInfo.piece = piece;
                _moveInfo.GetAvailableMoves(_grid, movementRule);
            }
            else return;
        }
        RequestHighlightRefresh(_moveInfo);
    }


    public void OnSquareSelectionChanged(Square square)
    {
        if (!_gameController.InputAllowed) return;
        if (_moveInfo != null && _moveInfo.piece != null)
        {
            if (_moveInfo.availableDestinations.Contains(square.Address))
            {
                _moveInfo.square = square;
                ExecuteMove(ref _moveInfo);
            }
        }
    }


    private void ExecuteMove(ref MoveInfo moveInfo)
    {
        _grid[moveInfo.piece.Address.y, moveInfo.piece.Address.x].RemovePiece();
        _grid[moveInfo.square.Address.y, moveInfo.square.Address.x].SetPiece(moveInfo.piece);

        moveInfo.ExecuteMove(onComplete: _gameController.OnEndOfTurn);
        moveInfo = null;

        RequestHighlightRefresh(null);
    }


    /// <summary>Pass 'null' to disable highlight</summary>
    private void RequestHighlightRefresh(MoveInfo moveInfo)
    {
        var positions = new List<Vector3>();
        if (moveInfo != null)
        {
            positions.Add(AddressToWorldPosition(moveInfo.piece.Address));

            if (moveInfo.availableDestinations != null)
            {
                foreach (var address in moveInfo.availableDestinations)
                {
                    positions.Add(AddressToWorldPosition(address));
                }
            }
        }
        HighlightRefreshRequestEvent?.Invoke(positions);
    }


    private Vector3 AddressToWorldPosition(Vector2Int address) => _boardParameters.AddressToWorldPosition(address);


    System.Text.StringBuilder sbuilder = new System.Text.StringBuilder();
    public void DebugLogGrid()
    {
        sbuilder.Clear();
        for (int y = _grid.GetLength(0) - 1; y >= 0; y--)
        {
            for (int x = 0; x < _grid.GetLength(1); x++)
            {
                sbuilder.Append(_grid[y, x].IsOccupied ? "O " : " _ ");
            }
            sbuilder.AppendLine();
        }
        Debug.Log(sbuilder);
    }
}

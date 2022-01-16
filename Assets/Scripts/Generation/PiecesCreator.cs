using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesCreator : MonoBehaviour
{
    [SerializeField] Piece _whitePiecePrefab;
    [SerializeField] Piece _blackPiecePrefab;

    BoardParameters _boardParams;


    public (List<Piece> player1, List<Piece> player2) CreatePlayersPieces(BoardParameters boardParams)
    {
        _boardParams = boardParams;

        var container = new GameObject("Pieces").transform;
        container.SetParent(transform);

        (List<Piece>, List<Piece>) playersPieces;

        var rotation = Quaternion.identity;
        playersPieces.Item1 = DistributePiecesToPlayer(container, _whitePiecePrefab, rotation);

        rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        playersPieces.Item2 = DistributePiecesToPlayer(container, _blackPiecePrefab, rotation);

        return playersPieces;
    }


    /// <summary>'rotation' is applied to vector pointing from the center of the board to its bottom left corner</summary>
    List<Piece> DistributePiecesToPlayer(Transform container, Piece piecePrefab, Quaternion rotation)
    {
        var pieces = new List<Piece>();

        var playerContainer = new GameObject(piecePrefab.name).transform;
        playerContainer.SetParent(container);

        for (int z = 0; z < _boardParams.PieceCountPerPlayer; z++)
        {
            for (int x = 0; x < _boardParams.PieceCountPerPlayer; x++)
            {
                var pos = rotation * (_boardParams.BottomLeftSquareCenter + new Vector3(x, 0, z) * _boardParams.SquareSize);
                var piece = Instantiate(piecePrefab, pos, Quaternion.identity, playerContainer);
                //piece.SetDependencies(_boardParams);
                piece.SetAddress(_boardParams.AddressFromWorldPosition(pos));
                pieces.Add(piece);
            }
        }

        return pieces;
    }
}

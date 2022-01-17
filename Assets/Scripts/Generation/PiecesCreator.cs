using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesCreator : MonoBehaviour
{
    [SerializeField] Piece _whitePiecePrefab;
    [SerializeField] Piece _blackPiecePrefab;

    BoardParameters _boardParams;

    public (List<Piece> player1, List<Piece> player2) PopulateCamps(Camp camp1, Camp camp2, BoardParameters boardParams)
    {
        _boardParams = boardParams;

        var container = new GameObject("Pieces").transform;
        container.SetParent(transform);

        (List<Piece>, List<Piece>) playersPieces;
        playersPieces.Item1 = PopulateCamp(container, camp1, _whitePiecePrefab);
        playersPieces.Item2 = PopulateCamp(container, camp2, _blackPiecePrefab);
        return playersPieces;
    }

    private List<Piece> PopulateCamp(Transform container, Camp camp, Piece piecePrefab)
    {
        var pieces = new List<Piece>();
        var playerContainer = new GameObject(piecePrefab.name).transform;
        playerContainer.SetParent(container);
        foreach (var square in camp.Squares)
        {
            var pos = _boardParams.AddressToWorldPosition(square.Address);
            var piece = Instantiate(piecePrefab, pos, Quaternion.identity, playerContainer);
            piece.Init(_boardParams.AddressFromWorldPosition(pos), _boardParams);
            piece.AssignOwner(camp.Owner);
            pieces.Add(piece);

        }
        return pieces;
    }
}

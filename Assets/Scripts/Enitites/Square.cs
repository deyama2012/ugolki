using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] Vector2Int _address;
    [SerializeField] Piece _occupiedBy;

    public Vector2Int Address => _address;

    public bool IsOccupied => _occupiedBy != null;

    public Piece OccupiedBy => _occupiedBy;

    public static event Action<Square> SquareClickedEvent;

    public void AssignAddress(Vector2Int address) => _address = address;

    public void SetPiece(Piece piece) => _occupiedBy = piece;

    public void RemovePiece() => _occupiedBy = null;

    private void OnMouseDown() => SquareClickedEvent?.Invoke(this);
}

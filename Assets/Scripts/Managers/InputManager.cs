using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Board _board;

    void Awake()
    {
        Piece.PieceClickedEvent += PieceClicked_EventHandler;
        Square.SquareClickedEvent += SquareClicked_EventHandler;
    }

    private void SquareClicked_EventHandler(Square square)
    {
        _board.OnSquareSelectionChanged(square);
    }

    private void PieceClicked_EventHandler(Piece piece)
    {
        _board.OnPieceSelectionChanged(piece);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _board.OnPieceSelectionChanged(null);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _board.DebugLogGrid();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] Square _whiteSquarePrefab;
    [SerializeField] Square _blackSquarePrefab;
    [SerializeField] BoardBorderSegment _borderSegmentPrefab;
    [SerializeField] GameObject _borderCornerPrefab;

    BoardParameters _boardParams;

    public static List<string> numbers;
    public static List<string> letters;


    public void GenerateBoard(Square[,] grid, BoardParameters boardParameters)
    {
        _boardParams = boardParameters;

        numbers = new List<string>(Enumerable.Range(1, _boardParams.SquaresPerSide).Select(i => i.ToString()));
        letters = new List<string>(Enumerable.Range(65, _boardParams.SquaresPerSide).Select(i => ((char)i).ToString()));

        GenerateSquares(grid);
        GenerateBorders();
    }


    private void GenerateSquares(Square[,] grid)
    {
        var squaresContainer = new GameObject("Squares").transform;
        squaresContainer.SetParent(transform);

        Square[] prefabs = { _whiteSquarePrefab, _blackSquarePrefab };

        for (int z = 0; z < _boardParams.SquaresPerSide; z++)
        {
            for (int x = 0; x < _boardParams.SquaresPerSide; x++)
            {
                int extraOffset = _boardParams.SquaresPerSide % 2 == 0 ? z % 2 : 0;    // Switch starting color if number of squares per board side is even 
                int prefabIndex = (z * _boardParams.SquaresPerSide + x + extraOffset) % 2;

                var pos = _boardParams.AddressToWorldPosition(new Vector2Int(x, z));
                var square = Instantiate(prefabs[prefabIndex], pos, Quaternion.identity, squaresContainer);
                square.AssignAddress(new Vector2Int(x, z));

                grid.Set(square.Address, square);
            }
        }
    }


    private void GenerateBorders()
    {
        var borderContainer = new GameObject("Border").transform;
        borderContainer.SetParent(transform);

        GenerateBorder_SingleSide(borderContainer, BoardBorderSegment.Orientation.Top, letters);
        GenerateBorder_SingleSide(borderContainer, BoardBorderSegment.Orientation.Bottom, letters);
        GenerateBorder_SingleSide(borderContainer, BoardBorderSegment.Orientation.Left, numbers);
        GenerateBorder_SingleSide(borderContainer, BoardBorderSegment.Orientation.Right, numbers);

        GenerateBorder_Corners(borderContainer);
    }


    private void GenerateBorder_SingleSide(Transform container, BoardBorderSegment.Orientation orientation, List<string> labels)
    {
        Vector3 boardBottomLeftSquareCorner = -_boardParams.TotalSize * 0.5f;

        for (int i = 0; i < _boardParams.SquaresPerSide; i++)
        {
            Vector3 offset = Vector3.zero;
            switch (orientation)
            {
                case BoardBorderSegment.Orientation.Top:
                    offset.x = i * _boardParams.SquareSize + _boardParams.SquareSize * 0.5f;
                    offset.z = _boardParams.TotalSize.z;
                    break;
                case BoardBorderSegment.Orientation.Bottom:
                    offset.x = i * _boardParams.SquareSize + _boardParams.SquareSize * 0.5f;
                    offset.z = 0;
                    break;
                case BoardBorderSegment.Orientation.Right:
                    offset.x = _boardParams.TotalSize.x;
                    offset.z = i * _boardParams.SquareSize + _boardParams.SquareSize * 0.5f;
                    break;
                case BoardBorderSegment.Orientation.Left:
                    offset.x = 0;
                    offset.z = i * _boardParams.SquareSize + _boardParams.SquareSize * 0.5f;
                    break;
            }

            Vector3 segmentPos = boardBottomLeftSquareCorner + offset;
            var segment = Instantiate(_borderSegmentPrefab, segmentPos, Quaternion.identity, container);
            segment.SetOrientation(orientation);
            segment.SetLabelText(labels[i]);
        }
    }


    private void GenerateBorder_Corners(Transform container)
    {
        Vector3 centerToTopLeftCorner = new Vector3(-1, 0, 1) * _boardParams.SquaresPerSide * _boardParams.SquareSize * 0.5f;

        var euler = Vector3.zero;
        for (int i = 0; i < 4; i++)
        {
            var rot = Quaternion.Euler(euler);
            var pos = rot * centerToTopLeftCorner;
            Instantiate(_borderCornerPrefab, pos, rot, container);
            euler.y += 90;
        }
    }
}

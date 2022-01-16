using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] Vector2Int _address;

    public Vector2Int Address => _address;
    public static event Action<Square> SquareClickedEvent;

    public void SetAddress(Vector2Int address)
    {
        _address = address;
    }

    private void OnMouseDown()
    {
        SquareClickedEvent?.Invoke(this);
    }
}

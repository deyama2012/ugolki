using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] Vector2Int _address;
    [SerializeField] Player _owner;
    [SerializeField] float _moveDuration = 0.5f;

    BoardParameters _boardParams;

    public Vector2Int Address => _address;
    public Player Owner => _owner;

    public static event Action<Piece> PieceClickedEvent;

    //public void SetDependencies(BoardParameters boardParameters) => _boardParams = boardParameters;

    public void SetAddress(Vector2Int address) => _address = address;

    public void SetOwner(Player player) => _owner = player;

    private void OnMouseDown() => PieceClickedEvent?.Invoke(this);

    //public Vector3 AddressToWorldPosition => _boardParams.AddressToWorldPosition(_address); //.BottomLeftSquareCenter + new Vector3(_address.x, 0, _address.y) * _boardParams.SquareSize;

    public void Move(Square square, Action onComplete)
    {
        _address = square.Address;
        StartCoroutine(MoveCoroutine(square.transform.position, _moveDuration, onComplete));
    }

    IEnumerator MoveCoroutine(Vector3 destination, float duration, Action onComplete)
    {
        float t = 0;
        var startPos = transform.position;
        while (t <= duration)
        {
            transform.position = Vector3.Lerp(startPos, destination, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;
        onComplete?.Invoke();
    }
}

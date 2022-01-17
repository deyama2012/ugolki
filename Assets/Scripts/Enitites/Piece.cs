using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    BoardParameters _boardParams;
    Player _owner;

    [SerializeField] Vector2Int _address;
    [SerializeField] float _moveDuration = 0.2f;
    [SerializeField] float _jumpHeight = 0.5f;

    public Vector2Int Address => _address;
    public Player Owner => _owner;

    /// <summary>Actual distance won't exceed 1.4142 for immediate diagonal neighbours, everything above that means we are more than 1 square away</summary>
    const float MAX_DISTANCE_TO_IMMEDIATE_NEIGHBOUR = 1.5f;

    public static event Action<Piece> PieceClickedEvent;


    public void Init(Vector2Int address, BoardParameters boardParameters)
    {
        _address = address;
        _boardParams = boardParameters;
    }


    public void AssignOwner(Player player) => _owner = player;


    public void Move(List<Vector2Int> waypoints, Action onComplete)
    {
        _address = waypoints[waypoints.Count - 1];
        StartCoroutine(MoveCoroutine(waypoints, _moveDuration, onComplete));
    }


    private IEnumerator MoveCoroutine(List<Vector2Int> waypoints, float duration, Action onComplete)
    {
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            float t = 0;
            var current = _boardParams.AddressToWorldPosition(waypoints[i]);
            var next = _boardParams.AddressToWorldPosition(waypoints[i + 1]);

            // If waypoints are not right next to each other, then we need to do parabolic trajectory (to jump over a piece)
            bool parabola = Vector2Int.Distance(waypoints[i], waypoints[i + 1]) > MAX_DISTANCE_TO_IMMEDIATE_NEIGHBOUR;

            float normalizedTime = 0;
            while (normalizedTime <= 1)
            {
                if (parabola)
                {
                    float yOffset = _jumpHeight * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                    transform.position = Vector3.Lerp(current, next, normalizedTime) + yOffset * Vector3.up;
                }
                else
                {
                    transform.position = Vector3.Lerp(current, next, normalizedTime);
                }

                t += Time.deltaTime;
                normalizedTime = t / duration;
                yield return null;
            }
        }
        transform.position = _boardParams.AddressToWorldPosition(waypoints[waypoints.Count - 1]);
        onComplete?.Invoke();
    }


    void OnMouseDown() => PieceClickedEvent?.Invoke(this);
}

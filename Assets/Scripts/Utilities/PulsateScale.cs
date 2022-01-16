using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateScale : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _minScale;
    [SerializeField] float _maxScale;

    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(_minScale, _maxScale, Mathf.Sin(Time.time * _speed) * 0.5f + 0.5f);
    }
}

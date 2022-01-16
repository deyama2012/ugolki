using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBorderSegment : MonoBehaviour
{
    public enum Orientation { Top, Right, Bottom, Left }

    [SerializeField] Transform _rotationPivot;
    [SerializeField] TMPro.TextMeshPro _textMesh;

    public void SetOrientation(Orientation orientation)
    {
        switch (orientation)
        {
            case Orientation.Top:
                _rotationPivot.eulerAngles = Vector3.zero;
                break;

            case Orientation.Right:
                _rotationPivot.eulerAngles = Vector3.up * 90;
                break;

            case Orientation.Bottom:
                _rotationPivot.eulerAngles = Vector3.up * 180;
                break;

            case Orientation.Left:
                _rotationPivot.eulerAngles = Vector3.up * 270;
                break;
        }
        var textMeshWorldEuler = _textMesh.rectTransform.eulerAngles;
        textMeshWorldEuler.y = 0;
        _textMesh.rectTransform.eulerAngles = textMeshWorldEuler;
    }

    public void SetLabelText(string labelText)
    {
        _textMesh.text = labelText;
    }
}

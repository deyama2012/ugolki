using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    [SerializeField] GameObject _highlightPrefab;

    List<GameObject> _highlightInstances = new List<GameObject>();


    private void Awake()
    {
        Board.HighlightRefreshRequestEvent += BoardHighlightRefreshRequest_EventHandler;
    }

    private void BoardHighlightRefreshRequest_EventHandler(List<Vector3> positions)
    {
        // Check how many additional instances we need to spawn, if any
        int extraSpawnCount = Mathf.Clamp(positions.Count - _highlightInstances.Count, 0, positions.Count);

        for (int i = 0; i < extraSpawnCount; i++)
        {
            _highlightInstances.Add(Instantiate(_highlightPrefab, transform));
        }

        for (int i = 0; i < _highlightInstances.Count; i++)
        {
            if (i < positions.Count)
            {
                _highlightInstances[i].SetActive(true);
                _highlightInstances[i].transform.position = positions[i];
            }
            else
            {
                _highlightInstances[i].SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        Board.HighlightRefreshRequestEvent -= BoardHighlightRefreshRequest_EventHandler;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Camp _camp;

    [SerializeField] string _name;
    [SerializeField] int _turnCount;

    public string Name => _name;
    public int TurnCount => _turnCount;
    public Camp Camp => _camp;

    public void SetName(string name) => _name = name;

    public void AssignCamp(Camp camp) => _camp = camp;

    public void IncrementTurnCount()
    {
        _turnCount++;
    }
}

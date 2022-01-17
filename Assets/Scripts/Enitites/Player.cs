using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField] string _name;
    [SerializeField] int _turnCount;
    [SerializeField] Camp _camp;

    public string Name => _name;
    public int TurnCount => _turnCount;
    public Camp Camp => _camp;

    public Player(string name) => _name = name;

    public void AssignCamp(Camp camp) => _camp = camp;

    public void IncrementTurnCount()
    {
        _turnCount++;
    }
}

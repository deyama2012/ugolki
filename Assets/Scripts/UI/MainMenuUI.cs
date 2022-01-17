using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] MovementRule[] _movementRules;
    [SerializeField] TMP_Dropdown _movementRuleDropdown;

    int selectedRuleIndex;

    private void Start()
    {
        PopulateDropDown();
        Board.MovementRulesRequest += BoardMovementRulesRequest_EventHandler;
    }

    private void BoardMovementRulesRequest_EventHandler(Action<MovementRule> callback)
    {
        callback?.Invoke(_movementRules[selectedRuleIndex]);
    }

    private void PopulateDropDown()
    {
        _movementRuleDropdown.options.Clear();
        foreach (var rule in _movementRules)
        {
            _movementRuleDropdown.options.Add(new TMP_Dropdown.OptionData(rule.name));
        }
        _movementRuleDropdown.onValueChanged.AddListener(DropDownValueChanged_EventListener);
        _movementRuleDropdown.RefreshShownValue();
    }

    private void DropDownValueChanged_EventListener(int index)
    {
        selectedRuleIndex = index;
    }

    private void OnDestroy()
    {
        Board.MovementRulesRequest -= BoardMovementRulesRequest_EventHandler;
    }
}

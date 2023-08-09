using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Mana Manage
    [SerializeField] private int _maxManaCost = 100;
    [ReadOnly, SerializeField] private int _currentMana = 0;
    private UI_ManaGauge _manaGuage;

    const int _startMana = 35;

    private void Start()
    {
        _manaGuage = BattleManager.BattleUI.UI_manaGauge;
        ChangeMana(_startMana);
    }

    public bool ChangeMana(int value)
    {
       
        if (_maxManaCost <= _currentMana + value)
        {
            _currentMana += value;
            _currentMana = _maxManaCost;
            _manaGuage.DrawGauge(_maxManaCost, _currentMana);
            return true;
        }  
        else if (_currentMana < value)
        {
            Debug.Log("Not enough mana");
            return false;
        }
        else
        {
            _currentMana += value;
            _manaGuage.DrawGauge(_maxManaCost, _currentMana);
        }

        return true;
    }

    public bool CanUseMana(int value)
    {
        if (_currentMana >= value)
            return true;

        Debug.Log("not enough mana");
        return false;
    }
}
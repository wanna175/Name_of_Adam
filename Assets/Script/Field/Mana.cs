using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Mana Manage
    [SerializeField] private int _maxMana = 100;
    [ReadOnly, SerializeField] private int _currentMana = 0;

    const int _startMana = 50;

    private void Start()
    {
        ChangeMana(_startMana);
    }

    public void ChangeMana(int value)
    {
        if (_currentMana + value > _maxMana)
        {
            _currentMana = _maxMana;
        }  
        else if (_currentMana + value < 0)
        {
            Debug.Log("Not enough mana");
        }
        else
        {
            _currentMana += value;
        }

        ManaInableCheck();
        BattleManager.BattleUI.UI_manaGauge.DrawGauge(_maxMana, _currentMana);
    }

    public void ManaInableCheck()
    {
        BattleManager.BattleUI.UI_playerSkill.InableCheck(_currentMana);
        BattleManager.BattleUI.UI_hands.InableCheck(_currentMana);
    }

    public bool CanUseMana(int value)
    {
        if (_currentMana >= value)
            return true;

        Debug.Log("not enough mana");
        return false;
    }
}
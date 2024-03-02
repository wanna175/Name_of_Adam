using System.Collections;
using Unity.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Mana Manage
    [SerializeField] private int _maxMana = 100;
    [ReadOnly, SerializeField] private int _currentMana = 0;

    private int _startMana;

    private void Start()
    {
        if (GameManager.OutGameData.IsUnlockedItem(15))
        {
            _startMana = 60;
        }
        else if (GameManager.OutGameData.IsUnlockedItem(12))
        {
            _startMana = 55;
        }
        else
        {
            _startMana = 50;
        }

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
        BattleManager.BattleUI.UI_playerSkill.InableCheck();
        BattleManager.BattleUI.UI_hands.InableCheck();
    }

    public bool CanUseMana(int value)
    {
        if (_currentMana >= value)
            return true;

        return false;
    }
}
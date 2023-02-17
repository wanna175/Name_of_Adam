using System.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    // Mana Manage
    [SerializeField] private int _maxManaCost = 200;
    [ReadOnly, SerializeField] private int _currentMana;
    private UI_ManaGuage _manaGuage;

    private void Awake()
    {
        _currentMana = _maxManaCost;
        _manaGuage = GameManager.UI.ShowScene<UI_ManaGuage>();
        _manaGuage.DrawGauge(_currentMana);
    }

    public void SetManaGuage(UI_ManaGuage guage)
    {
        _manaGuage = guage;
        _manaGuage.DrawGauge(_currentMana);
    }

    public void ChangeMana(int value)
    {
        _currentMana += value;

        if (_maxManaCost <= _currentMana)
            _currentMana = _maxManaCost;
        else if (_currentMana < 0)
            _currentMana = 0;

        _manaGuage.DrawGauge(_currentMana);
    }

    public bool CanUseMana(int value)
    {
        if (_currentMana >= value)
            return true;

        Debug.Log("not enough mana");
        return false;
    }
}
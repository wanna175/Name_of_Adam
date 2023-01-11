using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager
{
    ManaGuage _manaGuage;

    #region MaxManaCost
    private const int _MaxManaCost = 10;
    public int MaxManaCost => _MaxManaCost;
    #endregion
    #region ManaCost
    private int _ManaCost = 0;
    public int ManaCost => _ManaCost;
    #endregion

    public void InitMana()
    {
        _ManaCost = 0;
    }

    public void SetManaGuage(ManaGuage _guage)
    {
        _manaGuage = _guage;
    }

    public void AddMana(int value)
    {
        if (10 <= _ManaCost + value)
            _ManaCost = 10;
        else
            _ManaCost += value;

        _manaGuage.DrawGauge();
    }

    public bool UseMana(int value)
    {
        if (_ManaCost >= value)
        {
            _ManaCost -= value;
            _manaGuage.DrawGauge();

            return true;
        }
        else
        {
            return false;
        }
    }
}

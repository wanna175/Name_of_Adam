using System.Collections;
using UnityEngine;

public class UnitHP : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;

    public void InitHP(int maxHP)
    {
        _maxHP = maxHP;
        _currentHP = maxHP;
    }

    public void ChangeHP(int value)
    {
        _currentHP += value;

        if(_currentHP < 0)
        {
            _currentHP = 0;
            // Add Die Event
        }
        else if (_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }

        // Add HP Change Event
    }

    public int GetCurrentHP()
    {
        return _currentHP;
    }
}
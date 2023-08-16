using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    protected BattleUnit _unit;
    public void Init(BattleUnit unit)
    {
        _unit = unit;
    }

    public virtual void ActionStart(List<BattleUnit> hits)
    {
        BattleManager.Instance.AttackStart(_unit, hits);
    }

    public virtual void Action(BattleUnit receiver)
    {
        _unit.Attack(receiver, _unit.BattleUnitTotalStat.ATK);
    }
}
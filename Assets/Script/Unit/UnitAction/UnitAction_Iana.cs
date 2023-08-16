using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAction_Iana : UnitAction
{
    public override void ActionStart(List<BattleUnit> hits)
    {
        List<BattleUnit> targetList = new();

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            Debug.Log("UNIT");
            if (unit.Buff.CheckBuff(BuffEnum.TraceOfSolar))
            {
                Debug.Log("SOL");
                targetList.Add(unit);
            }
        }

        if (targetList.Count == 0)
        {
            Debug.Log("ZERO");
            BattleManager.Instance.AttackStart(_unit, hits);
        }
        else
        {
            BattleManager.Instance.AttackStart(_unit, targetList);
        }
    }

    public override void Action(BattleUnit receiver)
    {
        _unit.Attack(receiver, _unit.BattleUnitTotalStat.ATK);
    }
}
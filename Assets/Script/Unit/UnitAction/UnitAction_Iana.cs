using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_Iana : UnitAction
{
    public override void ActionStart(List<BattleUnit> hits)
    {
        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Buff.CheckBuff(BuffEnum.TraceOfSolar))
            {
                hits.Add(unit);
            }
        }
        
        BattleManager.Instance.AttackStart(_unit, hits.Distinct().ToList());
    }

    public override void Action(BattleUnit receiver)
    {
        _unit.Attack(receiver, _unit.BattleUnitTotalStat.ATK);
    }
}
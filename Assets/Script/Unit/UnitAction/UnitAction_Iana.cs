using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_Iana : UnitAction
{
    public override void ActionStart(BattleUnit attackUnit, List<BattleUnit> hits)
    {
        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Buff.CheckBuff(BuffEnum.TraceOfSolar) && unit.Team != attackUnit.Team)
            {
                hits.Add(unit);
            }
        }
        
        BattleManager.Instance.AttackStart(attackUnit, hits.Distinct().ToList());
    }
}
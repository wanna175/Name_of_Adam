using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_Iana : UnitAction
{
    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count == 0)
            return false;

        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Buff.CheckBuff(BuffEnum.TraceOfSolar) && unit.Team != attackUnit.Team)
            {
                hits.Add(unit);
            }
        }
        
        BattleManager.Instance.AttackStart(attackUnit, hits.Distinct().ToList());
        return true;
    }
}
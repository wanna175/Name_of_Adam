using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_CenteredSplash : UnitAction
{
    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count == 0)
            return false;

        List<BattleUnit> units = BattleManager.Field.GetUnitsInRange(attackUnit.Location, attackUnit.GetAttackRange(), attackUnit.Team == Team.Player ? Team.Enemy : Team.Player);

        foreach (BattleUnit unit in hits)
        {
            units.Add(unit);
        }

        BattleManager.Instance.AttackStart(attackUnit, units.Distinct().ToList());
        return true;
    }
}
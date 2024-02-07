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

        List<BattleUnit> units = BattleManager.Field.GetUnitsInRange(attackUnit.Location, attackUnit.GetSplashRange(coord, attackUnit.Location), attackUnit.Team == Team.Player ? Team.Enemy : Team.Player);

        BattleManager.Instance.AttackStart(attackUnit, units.Distinct().ToList());
        return true;
    }
}
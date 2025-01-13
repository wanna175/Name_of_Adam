using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_Laser : UnitAction
{
    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 1)
            return false;

        int direction = attackUnit.Location.x - hits[0].Location.x > 0 ? -1 : 1;

        foreach (Vector2 vec in attackUnit.GetAttackRange())
        {
            if (vec.x * direction > 0)
            {
                BattleUnit unit = BattleManager.Field.GetUnit(vec + attackUnit.Location);

                if (unit != null && unit.Team != attackUnit.Team)
                {
                    hits.Add(unit);
                }
            }
        }

        BattleManager.Instance.AttackStart(attackUnit, hits.Distinct().ToList());
        return true;
    }

    public override List<Vector2> GetSplashRangeForField(BattleUnit unit, Tile targetTile, Vector2 caster)
    {
        List<Vector2> splashRangeList = new();
        Vector2 target = BattleManager.Field.GetCoordByTile(targetTile);
        int direction = caster.x - target.x > 0 ? -1 : 1;

        foreach (Vector2 vec in unit.GetAttackRange())
        {
            if (vec.x * direction > 0 && BattleManager.Field.IsInRange(vec + caster))
            {
                splashRangeList.Add(vec + caster);
            }
        }

        return splashRangeList;
    }
}
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

        for (int i = 1; i < 6; i++)
        {
            Vector2 vec = new(attackUnit.Location.x + (i * direction), attackUnit.Location.y);
            BattleUnit unit = BattleManager.Field.GetUnit(vec);

            if (unit != null && unit.Team != attackUnit.Team)
            {
                hits.Add(unit);
            }
        }

        BattleManager.Instance.AttackStart(attackUnit, hits.Distinct().ToList());
        return true;
    }

    public override List<Vector2> GetSplashRangeForField(BattleUnit unit, Vector2 target, Vector2 caster)
    {
        List<Vector2> splashRangeList = new();
        int direction = caster.x - target.x > 0 ? -1 : 1;

        for (int i = 1; i < 6; i++)
        {
            Vector2 vec = new(caster.x + (i * direction), caster.y);
            if (BattleManager.Field.IsInRange(vec))
                splashRangeList.Add(vec);
        }

        return splashRangeList;
    }
}
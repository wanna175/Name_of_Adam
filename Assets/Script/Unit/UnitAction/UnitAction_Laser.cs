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

        int direction = 1;

        if (attackUnit.Location.x - hits[0].Location.x > 0)
        {
            direction = -1;
        }

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
}
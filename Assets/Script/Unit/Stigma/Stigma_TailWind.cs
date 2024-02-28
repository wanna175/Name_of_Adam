using System.Collections.Generic;
using UnityEngine;

public class Stigma_TailWind : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<Vector2> areaCoords = new();
        for (int i = -2; i < 3; i++)
            areaCoords.Add(new Vector2(0, i));

        List<BattleUnit> targetUnits = BattleManager.Field.GetUnitsInRange(caster.Location, areaCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                unit.SetBuff(gameObject.AddComponent<Buff_Tailwind>());
            }
        }
    }
}
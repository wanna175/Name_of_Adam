using System.Collections.Generic;
using UnityEngine;

public class Stigma_TailWind : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<Vector2> areaCoords = new List<Vector2>();
        for (int i = -2; i < 3; i++)
            areaCoords.Add(new Vector2(0, i));

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location, areaCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                Buff_Tailwind tailwind = new();
                unit.SetBuff(tailwind);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class Stigma_TailWind : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);

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
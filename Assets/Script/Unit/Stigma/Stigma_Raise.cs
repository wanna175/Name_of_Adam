using System.Collections.Generic;
using UnityEngine;

public class Stigma_Raise : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                Buff_Raise raise = new();
                unit.SetBuff(raise);
                if (Tier == StigmaTier.Tier2)
                {
                    unit.SetBuff(raise);
                }
            }
        }
    }
}
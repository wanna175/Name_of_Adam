using System.Collections.Generic;
using UnityEngine;

public class Stigma_Raise : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);
        caster.SetBuff(new Buff_Raise());
        if (Tier == StigmaTier.Tier2)
            caster.SetBuff(new Buff_Raise());

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                unit.SetBuff(new Buff_Raise());
                if (Tier == StigmaTier.Tier2)
                    unit.SetBuff(new Buff_Raise());
            }
        }
    }
}
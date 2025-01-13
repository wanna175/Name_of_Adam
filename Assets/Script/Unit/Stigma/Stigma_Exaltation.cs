using System.Collections.Generic;
using UnityEngine;

public class Stigma_Exaltation : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location, caster.Team);

        caster.SetBuff(new Buff_AttackBoost());
        if (Tier == StigmaTier.Tier2)
            caster.SetBuff(new Buff_AttackBoost());

        foreach (BattleUnit unit in targetUnits)
        {
            unit.SetBuff(new Buff_AttackBoost());
            if (Tier == StigmaTier.Tier2)
                unit.SetBuff(new Buff_AttackBoost());
        }
    }
}
using System;
using UnityEngine;

public class Stigma_BloodBlessing : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_BloodBlessing bloodBlessing = new();
        caster.SetBuff(bloodBlessing, caster);

        if (Tier == StigmaTier.Tier1)
        {
            bloodBlessing.SetValue(10);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            bloodBlessing.SetValue(20);
        }
    }
}

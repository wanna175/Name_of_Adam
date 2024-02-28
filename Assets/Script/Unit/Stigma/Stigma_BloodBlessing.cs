using System;
using UnityEngine;

public class Stigma_BloodBlessing : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_BloodBlessing bloodBlessing = gameObject.AddComponent<Buff_Stigma_BloodBlessing>();
        caster.SetBuff(bloodBlessing);

        if (Tier == StigmaTier.Tier1)
        {
            bloodBlessing.SetValue(10);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            bloodBlessing.SetValue(15);
        }
    }
}

using System;
using UnityEngine;

public class Stigma_Regeneration : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Regeneration regeneration = new();
        caster.SetBuff(regeneration, caster);

        if (Tier == StigmaTier.Tier1)
        {
            regeneration.SetValue(5);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            regeneration.SetValue(10);
        }
    }
}

using System;
using UnityEngine;

public class Stigma_Immortal : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Immortal immortal = new();
        caster.SetBuff(immortal, caster);

        if (Tier == StigmaTier.Tier2)
        {
            caster.SetBuff(immortal, caster);
        }
    }
}

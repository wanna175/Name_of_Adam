using System;
using UnityEngine;

public class Stigma_Immortal : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Immortal immortal = gameObject.AddComponent<Buff_Immortal>();
        caster.SetBuff(immortal);

        if (Tier == StigmaTier.Tier2)
        {
            caster.SetBuff(immortal);
        }
    }
}

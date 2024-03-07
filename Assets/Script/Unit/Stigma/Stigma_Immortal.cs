using System;
using UnityEngine;

public class Stigma_Immortal : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Immortal immortal = new();
        caster.SetBuff(immortal);

        if (name.Contains("II"))
        {
            caster.SetBuff(immortal);
        }
    }
}

using System;
using UnityEngine;

public class Stigma_Dispel : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Dispel dispel = new();
        caster.SetBuff(dispel, caster);
    }
}

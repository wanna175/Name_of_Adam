using System;
using UnityEngine;

public class Stigma_Dispel : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Dispel dispel = new();
        caster.SetBuff(dispel, caster);
    }
}

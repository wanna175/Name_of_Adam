using System;
using UnityEngine;

public class Stigma_Martyrdom : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Martyrdom martyrdom = new();
        caster.SetBuff(martyrdom);
    }
}

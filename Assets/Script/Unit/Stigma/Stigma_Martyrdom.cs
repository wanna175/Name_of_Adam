using System;
using UnityEngine;

public class Stigma_Martyrdom : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Martyrdom martyrdom = new();
        caster.SetBuff(martyrdom, caster);
    }
}

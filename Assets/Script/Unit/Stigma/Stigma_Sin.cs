using System;
using UnityEngine;

public class Stigma_Sin : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Sin());
    }
}

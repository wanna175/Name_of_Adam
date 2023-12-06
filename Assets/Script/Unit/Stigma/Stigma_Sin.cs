using System;
using UnityEngine;

public class Stigma_Sin : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Sin sin = new();
        caster.SetBuff(sin);
    }
}

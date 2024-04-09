using System;
using UnityEngine;

public class Stigma_Sin : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Vice buff = new();

        for (int i = 0; i < 66; i++)
            caster.SetBuff(buff);
    }
}
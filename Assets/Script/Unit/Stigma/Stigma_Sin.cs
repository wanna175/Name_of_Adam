using System;
using UnityEngine;

public class Stigma_Sin : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Vice buff = new();

        caster.SetBuff(buff);
        caster.Buff.GetBuff(BuffEnum.Vice).SetValue(66);
    }
}
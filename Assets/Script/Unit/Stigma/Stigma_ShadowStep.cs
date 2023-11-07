using System;
using UnityEngine;

public class Stigma_ShadowStep : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_ShadowStep shadowStep = new();
        caster.SetBuff(shadowStep, caster);
    }
}

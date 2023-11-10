using System;
using UnityEngine;

public class Stigma_Misdeed : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Misdeed misdeed = new();
        caster.SetBuff(misdeed);
    }
}

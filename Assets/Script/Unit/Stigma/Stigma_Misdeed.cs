using System;
using UnityEngine;

public class Stigma_Misdeed : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Misdeed misdeed = new();
        caster.SetBuff(misdeed, caster);
    }
}

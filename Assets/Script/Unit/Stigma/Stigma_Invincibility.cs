using System;
using UnityEngine;

public class Stigma_Invincibility : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Invincibility invincibility = new();
        caster.SetBuff(invincibility);
    }
}

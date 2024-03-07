using System;
using UnityEngine;

public class Stigma_Invincible : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Invincible invincible = new();
        caster.SetBuff(invincible);

        if (name.Contains("II"))
        {
            caster.SetBuff(invincible);
        }
    }
}

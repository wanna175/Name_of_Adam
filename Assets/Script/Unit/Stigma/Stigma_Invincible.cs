using System;
using UnityEngine;

public class Stigma_Invincible : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Invincible invincible = gameObject.AddComponent<Buff_Invincible>();
        caster.SetBuff(invincible);

        if (Tier == StigmaTier.Tier2)
        {
            caster.SetBuff(invincible);
        }
    }
}

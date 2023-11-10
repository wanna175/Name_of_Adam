using System;
using UnityEngine;

public class Stigma_DeathStrike : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_DeathStrike deathStrike = new();
        caster.SetBuff(deathStrike);
    }
}

using System;
using UnityEngine;

public class Stigma_DeathStrike : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_DeathStrike deathStrike = new();
        caster.SetBuff(deathStrike, caster);
    }
}

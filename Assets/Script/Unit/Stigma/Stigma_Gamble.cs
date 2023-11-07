using System;
using UnityEngine;

public class Stigma_Gamble : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Gamble gamble = new();
        caster.SetBuff(gamble, caster);
    }
}

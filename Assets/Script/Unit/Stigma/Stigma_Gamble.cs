using System;
using UnityEngine;

public class Stigma_Gamble : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Gamble gamble = new();
        caster.SetBuff(gamble, caster);
    }
}

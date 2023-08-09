using System;
using UnityEngine;

public class Stigma_Repetance : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Repetance repetance = new();
        caster.SetBuff(repetance, caster);
    }
}

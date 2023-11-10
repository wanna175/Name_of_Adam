using System;
using UnityEngine;

public class Stigma_Repetance : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Repetance repetance = new();
        caster.SetBuff(repetance);
    }
}

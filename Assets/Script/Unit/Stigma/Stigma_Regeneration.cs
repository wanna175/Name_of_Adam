using System;
using UnityEngine;

public class Stigma_Regeneration : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Regeneration regeneration = new();
        caster.SetBuff(regeneration);

        if (name.Contains("II"))
        {
            regeneration.SetValue(10);
        }
        else
        {
            regeneration.SetValue(5);
        }
    }
}

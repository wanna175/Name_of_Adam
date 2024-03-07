using System;
using UnityEngine;

public class Stigma_BloodBlessing : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_BloodBlessing bloodBlessing = new();
        caster.SetBuff(bloodBlessing);

        if (name.Contains("II"))
        {
            bloodBlessing.SetValue(15);
        }
        else
        {
            bloodBlessing.SetValue(10);
        }
    }
}

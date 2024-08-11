using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Stigma_Sadism : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Sadism sadism = new();

        if (caster.Buff.CheckBuff(BuffEnum.Stigmata_Sadism))
            sadism = caster.Buff.GetBuff(BuffEnum.Stigmata_Sadism) as Buff_Stigma_Sadism;

        if (Tier == StigmaTier.Tier1)
        {
            sadism.SetValue(2);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            sadism.SetValue(3);
        }

        caster.SetBuff(sadism);
    }
}
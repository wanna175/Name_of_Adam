using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stigma_Sadism : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Sadism sadism = new();
        if (Tier == StigmaTier.Tier1)
        {
            sadism.SetValue(2);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            sadism.SetValue(4);
        }

        caster.SetBuff(sadism);
    }
}
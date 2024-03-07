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
        if (name.Contains("II"))
        {
            sadism.SetValue(3);
        }
        else
        {
            sadism.SetValue(2);
        }

        caster.SetBuff(sadism);
    }
}
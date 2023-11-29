using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (caster.Fall.GetCurrentFallCount() / 2 >= caster.BattleUnitTotalStat.FallCurrentCount)
        {
            caster.SetBuff(new Buff_Stigma_Berserker());
        }
    }
}

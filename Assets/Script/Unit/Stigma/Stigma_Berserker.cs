using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (caster.Fall.GetCurrentFallCount() >= caster.BattleUnitTotalStat.FallMaxCount / 2)
        {
            if (!caster.Buff.CheckBuff(BuffEnum.Berserker))
                caster.SetBuff(new Buff_Stigma_Berserker());
        }
    }
}

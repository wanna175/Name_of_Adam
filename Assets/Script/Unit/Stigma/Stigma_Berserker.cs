using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (!caster.Buff.CheckBuff(BuffEnum.Berserker))
            caster.ChangeFall(caster.BattleUnitTotalStat.FallMaxCount - 1);
        caster.SetBuff(new Buff_Stigma_Berserker());
    }
}

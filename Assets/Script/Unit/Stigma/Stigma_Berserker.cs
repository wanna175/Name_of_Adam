using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        int fallDownCount = caster.BattleUnitTotalStat.FallMaxCount - caster.BattleUnitTotalStat.FallCurrentCount - 1;
        caster.ChangeFall(fallDownCount, FallAnimMode.Off);
        caster.SetBuff(new Buff_Berserker());
    }
}

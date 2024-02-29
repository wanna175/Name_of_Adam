using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        int fallDownCount = caster.BattleUnitTotalStat.FallMaxCount - 1;
        for (int i = 0; i < fallDownCount; i++)
            caster.ChangeFall(1);
        caster.SetBuff(gameObject.AddComponent<Buff_Berserker>());
    }
}

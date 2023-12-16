using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (!caster.Buff.CheckBuff(BuffEnum.Berserker))
        {
            // 이후 최대 낙인 설정 코드 필요
            caster.SetBuff(new Buff_Berserker());
        }
    }
}

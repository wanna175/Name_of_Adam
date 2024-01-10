using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Charge : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Charge());
    }
}

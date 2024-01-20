using UnityEngine;
using System.Collections.Generic;

public class Stigma_Rebirth : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Rebirth());
        caster.SetBuff(new Buff_Vice());
    }
}
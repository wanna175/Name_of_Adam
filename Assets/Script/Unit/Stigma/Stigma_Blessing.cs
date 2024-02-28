using System;
using UnityEngine;

public class Stigma_Blessing : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(gameObject.AddComponent<Buff_Stigma_Blessing>());
    }
}

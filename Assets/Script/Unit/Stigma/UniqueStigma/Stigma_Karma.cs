using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Karma : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(gameObject.AddComponent<Buff_Karma>());
    }
}

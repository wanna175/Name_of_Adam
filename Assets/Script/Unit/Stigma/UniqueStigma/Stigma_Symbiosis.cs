using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Symbiosis : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(gameObject.AddComponent<Buff_Stigma_Symbiosis>());
    }
}

using UnityEngine;
using System.Collections.Generic;

public class Stigma_Rebirth : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(gameObject.AddComponent<Buff_Stigma_Rebirth>());
        caster.SetBuff(gameObject.AddComponent<Buff_Vice>());
    }
}
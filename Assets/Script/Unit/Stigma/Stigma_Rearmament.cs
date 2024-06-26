using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma_Rearmament : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (caster.IsDoneAttack == false)
            caster.SetBuff(new Buff_Stigma_Rearmament());
    }
}

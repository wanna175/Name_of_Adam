using UnityEngine;

public class Stigma_Rearmament : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Rearmament());
    }
}

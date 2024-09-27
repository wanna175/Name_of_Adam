using UnityEngine;

public class Stigma_RepeatingSurge : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_RepeatingSurge());
    }
}

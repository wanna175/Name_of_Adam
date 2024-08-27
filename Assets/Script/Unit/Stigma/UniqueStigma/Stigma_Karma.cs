using UnityEngine;

public class Stigma_Karma : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        for (int i = 0; i < 3; i++)
            caster.SetBuff(new Buff_Karma());
    }
}

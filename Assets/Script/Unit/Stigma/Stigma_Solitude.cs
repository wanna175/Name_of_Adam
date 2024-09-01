using UnityEngine;

public class Stigma_Solitude : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Solitude());
    }
}

using UnityEngine;

public class Stigma_Absorption : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Absorption absorption = new();
        caster.SetBuff(absorption, caster);
    }
}
using UnityEngine;

public class Stigma_Dust : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Dust dust = new();
        caster.SetBuff(dust);
    }
}
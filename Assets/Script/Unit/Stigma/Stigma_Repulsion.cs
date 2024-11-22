using UnityEngine;

public class Stigma_Repulsion : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Smite());
    }
}

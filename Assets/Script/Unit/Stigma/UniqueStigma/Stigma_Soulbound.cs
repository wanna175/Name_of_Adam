using UnityEngine;

public class Stigma_Soulbound : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Soulbound());
        caster.SetBuff(new Buff_Immortality());
    }
}

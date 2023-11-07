using UnityEngine;

public class Stigma_Expand : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Expand expand = new();
        caster.SetBuff(expand, caster);
    }
}
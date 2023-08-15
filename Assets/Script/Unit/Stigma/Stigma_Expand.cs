using UnityEngine;

public class Stigma_Expand : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Expand expand = new();
        caster.SetBuff(expand, caster);
    }
}
using UnityEngine;

public class Stigma_Hook : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Hook hook = new();  
        caster.SetBuff(hook, caster);
    }
}
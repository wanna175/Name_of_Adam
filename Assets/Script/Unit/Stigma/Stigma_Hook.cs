using UnityEngine;

public class Stigma_Hook : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_Hook hook = new();  
        caster.SetBuff(hook, caster);
    }
}
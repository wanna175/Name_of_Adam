using UnityEngine;

public class Stigma_BishopsPraise : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(gameObject.AddComponent<Buff_Stigma_BishopsPraise>());
    }
}
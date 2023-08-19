using UnityEngine;

public class Stigma_BishopsPraise : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_BishopsPraise bishopsPraise = new();
        caster.SetBuff(bishopsPraise, caster);
    }
}
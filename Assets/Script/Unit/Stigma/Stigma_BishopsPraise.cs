using UnityEngine;

public class Stigma_BishopsPraise : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_BishopsPraise bishopsPraise = new();
        caster.SetBuff(bishopsPraise);
    }
}
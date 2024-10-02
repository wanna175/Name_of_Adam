using UnityEngine;

public class Stigma_DeadlySin : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Malevolence buff = new();

        caster.SetBuff(buff);
        caster.Buff.GetBuff(BuffEnum.Malevolence).SetValue(66);
    }
}
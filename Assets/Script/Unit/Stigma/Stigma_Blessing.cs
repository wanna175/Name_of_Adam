using UnityEngine;

public class Stigma_Blessing : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_Blessing blessing = new();
        if (caster.Buff.CheckBuff(BuffEnum.Stigmata_Blessing))
            blessing = caster.Buff.GetBuff(BuffEnum.Stigmata_Blessing) as Buff_Stigma_Blessing;

        if (Tier == StigmaTier.Tier1)
        {
            blessing.SetValue(10);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            blessing.SetValue(15);
        }

        caster.SetBuff(blessing);
    }
}

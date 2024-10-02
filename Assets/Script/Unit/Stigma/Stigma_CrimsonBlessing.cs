using UnityEngine;

public class Stigma_CrimsonBlessing : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_CrimsonBlessing crimsonBlessing = new();
        caster.SetBuff(crimsonBlessing);

        if (Tier == StigmaTier.Tier1)
        {
            crimsonBlessing.SetValue(10);
        }
        else if (Tier == StigmaTier.Tier2)
        {
            crimsonBlessing.SetValue(15);
        }
    }
}

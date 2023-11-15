using UnityEngine;

public class Stigma_LunarEclipse : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_LunarEclipse lunarEclipse = new();
        caster.SetBuff(lunarEclipse);
    }
}
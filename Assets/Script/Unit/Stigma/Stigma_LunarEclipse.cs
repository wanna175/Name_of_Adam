using UnityEngine;

public class Stigma_LunarEclipse : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_LunarEclipse lunarEclipse = new();
        caster.SetBuff(lunarEclipse, caster);
    }
}
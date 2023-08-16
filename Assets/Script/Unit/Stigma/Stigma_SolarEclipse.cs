using UnityEngine;

public class Stigma_SolarEclipse : Stigma
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        Buff_Stigma_SolarEclipse solarEclipse = new();
        caster.SetBuff(solarEclipse, caster);
    }
}
using UnityEngine;

public class Stigma_SolarEclipse : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        Buff_Stigma_SolarEclipse solarEclipse = new();
        caster.SetBuff(solarEclipse);
    }
}
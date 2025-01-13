using UnityEngine;

public class Stigma_Symbiosis : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Symbiosis());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(1);

            Stat buffedStat = new();

            buffedStat.MaxHP = 30;
            buffedStat.CurrentHP = 30;

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}

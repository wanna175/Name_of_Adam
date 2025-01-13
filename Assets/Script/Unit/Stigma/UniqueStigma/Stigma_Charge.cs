using UnityEngine;

public class Stigma_Charge : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Charge());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(1);

            Stat buffedStat = new();

            buffedStat.MaxHP = 5;
            buffedStat.CurrentHP = 5;

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}

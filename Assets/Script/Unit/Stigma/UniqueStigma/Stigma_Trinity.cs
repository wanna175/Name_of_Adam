using UnityEngine;

public class Stigma_Trinity : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Trinity());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(2);

            Stat buffedStat = new();

            buffedStat.MaxHP = 50;
            buffedStat.CurrentHP = 50;

            buffedStat.ATK = 3;

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}

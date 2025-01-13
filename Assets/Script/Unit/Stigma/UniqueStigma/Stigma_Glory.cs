using UnityEngine;

public class Stigma_Glory : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Glory());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(3);

            Stat buffedStat = new();

            buffedStat.MaxHP = 25;
            buffedStat.CurrentHP = 25;

            buffedStat.ATK = 5;

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}
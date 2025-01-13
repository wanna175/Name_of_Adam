using UnityEngine;

public class Stigma_ThornsOfOblivion : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_ThornsOfOblivion());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(3);

            Stat buffedStat = new();

            buffedStat.MaxHP = 60;
            buffedStat.CurrentHP = 60;

            buffedStat.ATK = 5;

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}

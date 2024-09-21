using UnityEngine;

public class Stigma_Dusk : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Dusk());

        if (caster.Team == Team.Enemy && !caster.Buff.CheckBuff(BuffEnum.Edified))
        {
            Buff_EliteStatBuff statBuff = new();

            statBuff.SetValue(2);

            Stat buffedStat = new();

            if (caster.Data.ID == "엘리우스")
            {
                buffedStat.MaxHP = 20;
                buffedStat.CurrentHP = 20;
            }
            else if (caster.Data.ID == "야나")
            {
                buffedStat.MaxHP = 5;
                buffedStat.CurrentHP = 5;
            }

            statBuff.SetStat(buffedStat);

            caster.SetBuff(statBuff);
        }
    }
}
using UnityEngine;

public class Stigma_Berserker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        int fallDownCount = caster.BattleUnitTotalStat.FallMaxCount - caster.BattleUnitTotalStat.FallCurrentCount - 1;
        caster.ChangeFall(fallDownCount, null, FallAnimMode.Off);
        for (int i = 0; i < 5; i++)
            caster.SetBuff(new Buff_AttackBoost());
        //caster.SetBuff(new Buff_Berserker());
    }
}

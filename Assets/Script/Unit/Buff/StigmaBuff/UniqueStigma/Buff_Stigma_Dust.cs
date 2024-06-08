using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Dust : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Dust;

        _name = "황혼";

        _description = "황혼.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        int count = caster.Buff.GetBuffStack(BuffEnum.TraceOfDust);
        
        if (count >= 1)
        {
            List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);

            foreach (BattleUnit unit in targetUnits)
            {
                Debug.Log(unit);
                if (unit.Team == caster.Team)
                    unit.GetAttack(-_owner.BattleUnitTotalStat.ATK, null);
            }
        }
        if (count >= 2)
        {
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);
        }

        Buff_TraceOfDust dust = new();
        caster.SetBuff(dust);

        return false;
    }
}
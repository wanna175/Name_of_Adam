using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Mercy : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Mercy;

        _name = "ÀÚºñ";

        _description = "Mercy Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && caster.BattleUnitTotalStat.FallMaxCount / 2 >= caster.BattleUnitTotalStat.FallMaxCount - caster.Fall.GetCurrentFallCount())
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Cleanse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Cleanse;

        _name = "Cleanse";

        _description = "Cleanse Info";

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
        if (caster != null && _owner.Buff.GetHasBuffNum() >= 2)
            caster.ChangeFall(1, FallAnimMode.On);

        return false;
    }
}

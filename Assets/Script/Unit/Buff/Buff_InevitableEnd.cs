using UnityEngine;

public class Buff_InevitableEnd : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.InevitableEnd;

        _name = "鞘楷利 场";

        _description = "鞘楷利 场";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitDiedEvent();

        return false;
    }
}
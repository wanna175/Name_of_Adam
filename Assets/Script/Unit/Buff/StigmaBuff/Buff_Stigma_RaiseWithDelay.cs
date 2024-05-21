using UnityEngine;

public class Buff_Stigma_RaiseWithDelay : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.RaiseWithDelay;

        _name = "RaiseWithDelay";

        _description = "RaiseWithDelay Info";

        _count = -1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_owner.IsDoneAttack == false)
            _owner.SetBuff(new Buff_Raise());

        return false;
    }
}
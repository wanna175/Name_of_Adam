using UnityEngine;

public class Buff_Stigma_Rearmament : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Rearmament;

        _name = "Rearmament";

        _description = "Rearmament Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_owner.IsDoneAttack == false)
        {
            _owner.SetBuff(new Buff_Raise());
            _owner.SetBuff(new Buff_Tailwind());
        }

        return false;
    }
}
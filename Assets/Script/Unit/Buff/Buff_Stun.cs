using UnityEngine;

public class Buff_Stun : Buff
{    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stun;

        _name = "기절";

        _description = "기절.";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.ACTION_TURN_START;

        _statBuff = false;

        _dispellable = true;

        _caster = caster;

        _owner = owner;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        return true;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
using UnityEngine;

public class Buff_AfterAttackDead : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AfterAttackDead;

        _name = "AfterAttackDead";

        _description = "AfterAttackDead Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _isSystemBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_owner != null)
        {
            _owner.UnitDiedEvent();
            _owner.UnitRenderer.color = new(1, 1, 1, 1);
        }

        return false;
    }
}
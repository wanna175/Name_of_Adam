using UnityEngine;

public class Buff_AfterAttackDead : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AfterAttackDead;

        _name = "���� �� ���";

        _description = "���� �� ���";

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

        if (_owner != null)
        {
            _owner.UnitRenderer.color = new(1, 1, 1, 1);
        }

        return false;
    }
}
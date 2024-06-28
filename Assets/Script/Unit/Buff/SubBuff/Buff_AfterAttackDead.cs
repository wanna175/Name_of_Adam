using UnityEngine;

public class Buff_AfterAttackDead : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AfterAttackDead;

        _name = "°ø°Ý ÈÄ »ç¸Á";

        _description = "°ø°Ý ÈÄ »ç¸Á";

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
        if (_owner != null)
        {
            _owner.UnitDiedEvent();
            _owner.UnitRenderer.color = new(1, 1, 1, 1);
        }

        return false;
    }
}
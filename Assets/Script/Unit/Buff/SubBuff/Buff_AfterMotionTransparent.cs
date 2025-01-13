using UnityEngine;

public class Buff_AfterMotionTransparent : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AfterMotionTransparent;

        _name = "AfterMotionTransparent";

        _description = "AfterMotionTransparent Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_MOTION_END;

        _buffActiveTiming = ActiveTiming.ATTACK_MOTION_END;

        _owner = owner;

        _isSystemBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitRenderer.color = new(0, 0, 0, 0);

        return false;
    }
}
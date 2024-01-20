using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Rebirth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Rebirth;

        _name = "환생";

        _description = "환생.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.ATTACK_MOTION_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.SetBuff(new Buff_AfterMotionTransparent());
        _owner.SetBuff(new Buff_AfterAttackBounce());

        return false;
    }
}
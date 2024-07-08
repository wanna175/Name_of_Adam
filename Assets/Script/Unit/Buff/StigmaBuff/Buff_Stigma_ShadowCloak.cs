using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_ShadowCloak: Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.ShadowCloak;

        _name = "ShadowCloak";

        _description = "ShadowCloak Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (RandomManager.GetFlag(0.3f))
            return true;
        else
            return false;
    }
}
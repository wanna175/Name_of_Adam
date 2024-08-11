using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_ShadowCloak: Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_ShadowCloak;

        _name = "ShadowCloak";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (RandomManager.GetFlag(0.3f))
            return true;
        else
            return false;
    }
}
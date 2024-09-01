using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Rebirth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Rebirth;

        _name = "Rebirth";

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.SetBuff(new Buff_AfterMotionTransparent());
        _owner.SetBuff(new Buff_AfterAttackBounce());

        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Advent : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Advent;

        _name = "Advent";

        _count = 1;

        _countDownTiming = ActiveTiming.TURN_END;

        _buffActiveTiming = ActiveTiming.TURN_END;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_count == 1)
        {
            _owner.UnitDiedEvent(false);
        }

        return false;
    }
}
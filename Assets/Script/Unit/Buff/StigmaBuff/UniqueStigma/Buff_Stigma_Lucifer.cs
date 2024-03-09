using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Lucifer : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Lucifer;

        _name = "루시퍼의 등장";

        _description = "루시퍼가 강림하였습니다.";

        _count = 1;

        _countDownTiming = ActiveTiming.TURN_END;

        _buffActiveTiming = ActiveTiming.TURN_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
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
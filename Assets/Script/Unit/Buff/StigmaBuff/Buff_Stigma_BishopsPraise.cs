using UnityEngine;

public class Buff_Stigma_BishopsPraise : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.BishopsPraise;

        _name = "주교의 축복";

        _description = "주교의 축복.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        bool[] moveRange = new bool[] {
            false, false, false, false, false,
            false, true, false, true, false,
            false, false, false, false, false,
            false, true, false, true, false,
            false, false, false, false, false
        };

        _owner.AddMoveRange(moveRange);

        return false;
    }
}
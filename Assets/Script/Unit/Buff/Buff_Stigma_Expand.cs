using UnityEngine;

public class Buff_Stigma_Expand : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Expand;

        _name = "확장";

        _description = "확장.";

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
            false, false, true, false, false,
            false, false, false, false, false,
            true, false, false, false, true,
            false, false, false, false, false,
            false, false, true, false, false
        };

        caster.AddMoveRange(moveRange);

        return false;
    }
}
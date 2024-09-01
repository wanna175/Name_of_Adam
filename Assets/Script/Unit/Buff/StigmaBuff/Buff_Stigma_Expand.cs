using UnityEngine;

public class Buff_Stigma_Expand : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Expand;

        _name = "Expand";

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _stigmataBuff = true;
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

        _owner.AddMoveRange(moveRange);

        return false;
    }
}
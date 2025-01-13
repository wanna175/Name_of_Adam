using UnityEngine;

public class Buff_Stigma_BishopsPraise : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_BishopsPraise;

        _name = "BishopsPraise";

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _stigmataBuff = true;
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
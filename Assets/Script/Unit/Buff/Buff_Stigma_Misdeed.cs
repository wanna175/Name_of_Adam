using UnityEngine;

public class Buff_Stigma_Misdeed : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Misdeed;

        _name = "學機";

        _description = "學機.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.UNIT_TERMINATE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.SetBuff(new Buff_Vice());

        return false;
    }
}
using UnityEngine;

public class Buff_Stigma_Teleport : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.None;

        _name = "";

        _description = "";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.UNIT_KILL;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {   
        _owner.SetBuff(new Buff_Teleport());

        return false;
    }
}
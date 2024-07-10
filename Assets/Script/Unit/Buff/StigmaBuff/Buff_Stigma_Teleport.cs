using UnityEngine;

public class Buff_Stigma_Teleport : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Teleport;

        _name = "Teleport";

        _description = "Teleport Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.UNIT_KILL | ActiveTiming.STIGMA;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.SetBuff(new Buff_SacredStep());

        return false;
    }
}
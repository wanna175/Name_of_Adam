using UnityEngine;

public class Buff_Stigma_Aid : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Aid;

        _name = "Aid";

        _description = "Aid Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.AFTER_SWITCH;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        caster.GetHeal(15, _owner);

        return false;
    }
}
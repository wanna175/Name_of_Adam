using UnityEngine;

public class Buff_Stigma_Martyrdom : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Martyrdom;

        _name = "순교";

        _description = "순교.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        if (_owner.BattleUnitTotalStat.FallMaxCount / 2 >= _owner.Fall.GetCurrentFallCount())
            receiver.ChangeFall(1);

        return false;
    }
}
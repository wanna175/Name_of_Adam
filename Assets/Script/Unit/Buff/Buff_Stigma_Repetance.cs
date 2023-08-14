using UnityEngine;

public class Buff_Stigma_Repetance : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Repetance;

        _name = "참회";

        _description = "참회.";

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
        if (_owner.BattleUnitTotalStat.MaxHP / 2 >= _owner.HP.GetCurrentHP())
            receiver.ChangeFall(1);

        return false;
    }
}
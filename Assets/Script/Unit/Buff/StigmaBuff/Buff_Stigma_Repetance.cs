using UnityEngine;

public class Buff_Stigma_Repetance : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Repetance;

        _name = "참회";

        _description = "해당 유닛의 체력이 절반 이하일때 피격 대상의 신앙을 1 떨어뜨립니다";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && _owner.BattleUnitTotalStat.MaxHP / 2 >= _owner.HP.GetCurrentHP())
            caster.ChangeFall(1);

        return false;
    }
}
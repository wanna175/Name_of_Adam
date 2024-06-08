using UnityEngine;

public class Buff_Stigma_Martyrdom : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Martyrdom;

        _name = "순교";

        _description = "해당 유닛의 교화게이지가 절반 이상 차있는 상태에서 공격 시 피격 유닛의 신앙을 1 떨어뜨립니다";

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
        if (caster != null && _owner.BattleUnitTotalStat.FallMaxCount - _owner.Fall.GetCurrentFallCount() <= 2)
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);

        return false;
    }
}
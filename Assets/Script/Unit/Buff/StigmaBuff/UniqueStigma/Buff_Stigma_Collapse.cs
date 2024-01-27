using UnityEngine;

public class Buff_Stigma_Collapse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Collapse;

        _name = "붕괴";

        _description = "오벨리스크는 아무런 행동도 하지 못하며, 신앙이 모두 떨어지거나, 바누엘이 사라지면 파괴됩니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitDiedEvent();

        return true;
    }
}
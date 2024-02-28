using UnityEngine;

public class Buff_Stigma_Sacrifice : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sacrifice;

        _name = "희생";

        _description = "구원자는 자신의 심복 또는 희생의 꽃이 죽을때마다 데미지를 입으며\n타락될때마다 신앙이 감소합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
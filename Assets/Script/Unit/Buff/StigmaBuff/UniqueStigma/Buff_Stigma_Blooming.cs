using UnityEngine;

public class Buff_Stigma_Blooming : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Blooming;

        _name = "개화";

        _description = "희생의 꽃은 개화하면 구원자의 심복이 됩니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
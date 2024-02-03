using UnityEngine;

public class Buff_Stigma_Trinity : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Trinity;

        _name = "삼위일체";

        _description = "매 턴 무기를 바꿔 사거리와 효과가 달라집니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
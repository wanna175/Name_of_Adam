using UnityEngine;

public class Buff_Stigma_LegacyOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.LegacyOfBabel;

        _name = "바벨의 유산";

        _description = "자신의 턴마다 공격 타일에 오벨리스크를 설치합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
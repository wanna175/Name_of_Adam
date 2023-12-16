using UnityEngine;

public class Buff_Stigma_WrathOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.WrathOfBabel;

        _name = "바벨의 진노";

        _description = "필드에 오벨리스크가 6개 이상이 되면 경우 오벨리스크가 있는 타일을 제외한 타일을 전부 공격합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
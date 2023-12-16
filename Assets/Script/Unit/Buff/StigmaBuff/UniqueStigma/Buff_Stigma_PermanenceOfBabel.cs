using UnityEngine;

public class Buff_Stigma_PermanenceOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.PermanenceOfBabel;

        _name = "바벨의 영속";

        _description = "적 유닛이 플레이어턴에 있던 타일을 공격하거나, 오벨리스크 주위의 적 유닛들을 공격합니다. \n니므롯이 공격할 타일은 플레이어턴부터 표시됩니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
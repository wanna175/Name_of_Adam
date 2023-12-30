using UnityEngine;

public class Buff_Stigma_Charge : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Charge;

        _name = "돌격";

        _description = "공격하려는 유닛과 투발카인 사이에 다른 유닛이 있을시 공격할 수 없습니다.\n두 칸 이상 떨어진 적을 공격할시 돌격합니다.\n돌격 시 두 배의 데미지를 주고 신앙을 떨어뜨리며 자신은 다음턴 행동할 수 없게 됩니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
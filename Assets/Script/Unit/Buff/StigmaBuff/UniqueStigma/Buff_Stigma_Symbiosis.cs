using UnityEngine;

public class Buff_Stigma_Symbiosis : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.LegacyOfBabel;

        _name = "공생";

        _description = "한 몸에 공존하는 라헬과 레아가 각각 턴을 갖습니다.\n라헬은 공격 시 적에게 짐승의 낙인을 부여하며 적의 배후로 이동합니다.\n레아는 이동할 수 없으며 범위 안의 적을 광역으로 공격합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
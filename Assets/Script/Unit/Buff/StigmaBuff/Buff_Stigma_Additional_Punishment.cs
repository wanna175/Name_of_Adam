using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Additional_Punishment : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Additional_Punishment;

        _name = "가중 처벌";

        _description = "공격 후 공격한 대상이 아닌 범위 안의 적 하나를 추가로 공격합니다.";

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
        // BattleManager-ActionPhaseClick에서 구현됨
        return false;
    }
}

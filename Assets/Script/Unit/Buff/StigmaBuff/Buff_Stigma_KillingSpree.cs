using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.KillingSpree ;

        _name = "척살";

        _description = "적 처치 시 추가 이동턴과 공격턴을 가집니다";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.UNIT_KILL;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team == Team.Enemy)
            BattleManager.Data.BattleOrderInsert(0, _owner);

        return false;
    }
}

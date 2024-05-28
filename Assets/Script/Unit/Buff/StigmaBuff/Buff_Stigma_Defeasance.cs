using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Defeasance : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Additional_Punishment;

        _name = "무효화";

        _description = "피격 시 20% 확률로 적 데미지를 무효화합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACKED;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (RandomManager.GetFlag(0.2f))
            return true;
        else
            return false;
    }
}
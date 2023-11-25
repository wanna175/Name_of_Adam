using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Assasination : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Assasination;

        _name = "암살";

        _description = "맵 어디든 소환 가능합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        // 이 버프는 BattleUnit이 아닌, BattleManager에서 직접 수행됩니다.

        return false;
    }
}

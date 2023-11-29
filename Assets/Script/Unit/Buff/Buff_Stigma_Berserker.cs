using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Berserker : Buff
{
    public override void Init(BattleUnit owner)
    {
        _name = "광화";

        _description = "피해량의 30퍼센트를 회복.";

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
        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += (int)(_owner.BattleUnitTotalStat.ATK * 0.5);

        return stat;
    }
}

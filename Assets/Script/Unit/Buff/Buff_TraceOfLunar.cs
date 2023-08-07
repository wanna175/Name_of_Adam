using UnityEngine;

public class Buff_TraceOfLunar: Buff
{    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.TraceOfLunar;

        _name = "崔狼 如利";

        _description = "崔狼 如利.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _statBuff = false;

        _dispellable = true;

        _caster = caster;

        _owner = owner;
    }
}
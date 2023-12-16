using UnityEngine;

public class Buff_Stigma_LunarEclipse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.LunarEclipse;

        _name = "월식";

        _description = "월식.";

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
        if (caster.Buff.CheckBuff(BuffEnum.TraceOfSolar))
        {
            caster.DeleteBuff(BuffEnum.TraceOfSolar);
            caster.ChangeFall(1);
        }
        else
        {
            Buff_TraceOfLunar traceOfLunar = new();
            caster.SetBuff(traceOfLunar);
        }

        return false;
    }
}
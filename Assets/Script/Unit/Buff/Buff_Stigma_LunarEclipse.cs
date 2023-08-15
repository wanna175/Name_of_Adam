using UnityEngine;

public class Buff_Stigma_LunarEclipse : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.LunarEclipse;

        _name = "월식";

        _description = "월식.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        if (receiver.Buff.CheckBuff(BuffEnum.TraceOfSolar))
        {
            receiver.Buff.DeleteBuff(BuffEnum.TraceOfSolar);
            receiver.ChangeFall(1);
        }
        else
        {
            Buff_TraceOfLunar traceOfLunar = new();
            receiver.SetBuff(traceOfLunar, caster);
        }

        return false;
    }
}
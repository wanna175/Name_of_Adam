using UnityEngine;

public class Buff_Stigma_SolarEclipse : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.SolarEclipse;

        _name = "일식";

        _description = "일식.";

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
        if (receiver.Buff.CheckBuff(BuffEnum.TraceOfLunar))
        {
            receiver.DeleteBuff(BuffEnum.TraceOfLunar);
            receiver.ChangeFall(1);

            caster.GetHeal(10, caster);
        }
        else
        {
            Buff_TraceOfSolar traceOfSolar = new();
            receiver.SetBuff(traceOfSolar, caster);
        }

        return false;
    }
}
using UnityEngine;

public class Buff_Stigma_Repetance : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Repetance;

        _name = "ÂüÈ¸";

        _description = "";

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
        if (caster != null && caster.BattleUnitTotalStat.MaxHP == caster.HP.GetCurrentHP())
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);

        return false;
    }
}
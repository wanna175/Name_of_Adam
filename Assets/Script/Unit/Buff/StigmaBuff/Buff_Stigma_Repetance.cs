using UnityEngine;

public class Buff_Stigma_Repetance : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Repetance;

        _name = "Repetance";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && caster.BattleUnitTotalStat.MaxHP == caster.HP.GetCurrentHP())
            caster.ChangeFall(1, _owner, FallAnimMode.On);

        return false;
    }
}
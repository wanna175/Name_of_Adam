using UnityEngine;

public class Buff_Stigma_Martyrdom : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Martyrdom;

        _name = "Martyrdom";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && _owner.BattleUnitTotalStat.FallMaxCount - _owner.Fall.GetCurrentFallCount() <= 2)
            caster.ChangeFall(1, FallAnimMode.On);

        return false;
    }
}
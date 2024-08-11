using UnityEngine;

public class Buff_Stigma_Collapse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Collapse;

        _name = "Collapse";

        _buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitDiedEvent();

        return true;
    }
}
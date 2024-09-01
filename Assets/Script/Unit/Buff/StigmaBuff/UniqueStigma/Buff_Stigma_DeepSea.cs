using UnityEngine;

public class Buff_Stigma_DeepSea : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_DeepSea;

        _name = "DeepSea";

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
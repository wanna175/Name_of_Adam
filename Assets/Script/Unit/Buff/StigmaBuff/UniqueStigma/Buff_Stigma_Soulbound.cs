using UnityEngine;

public class Buff_Stigma_Soulbound : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Soulbound;

        _name = "Soulbound";

        //_buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitDiedEvent();

        return true;
    }
}
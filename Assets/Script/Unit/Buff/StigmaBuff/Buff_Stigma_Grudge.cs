using UnityEngine;

public class Buff_Stigma_Grudge : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Grudge;

        _name = "Grudge";

        _buffActiveTiming = ActiveTiming.FIELD_UNIT_FALLED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team == _owner.Team && caster != _owner)
        {
            Buff_Grudge grudge = new();
            grudge.SetValue(20);
            _owner.SetBuff(grudge);
        }

        return false;
    }
}
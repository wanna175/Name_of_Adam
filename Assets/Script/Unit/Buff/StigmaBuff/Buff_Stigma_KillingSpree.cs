using UnityEngine;

public class Buff_Stigma_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_KillingSpree;

        _name = "KillingSpree";

        _buffActiveTiming = ActiveTiming.UNIT_KILL | ActiveTiming.TURN_END;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && caster.Team != _owner.Team && !IsActive)
        {
            _owner.SetBuff(new Buff_KillingSpree());
            IsActive = true;
        }
        else if (caster == null)
        {
            IsActive = false;
        }

        return false;
    }
}

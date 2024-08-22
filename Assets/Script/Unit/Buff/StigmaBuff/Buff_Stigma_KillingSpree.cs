using UnityEngine;

public class Buff_Stigma_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_KillingSpree;

        _name = "KillingSpree";

        _buffActiveTiming = ActiveTiming.UNIT_KILL;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team != _owner.Team)
        {
            _owner.SetBuff(new Buff_KillingSpree());
            _owner.SetBuff(new Buff_KillingSpree());
        }

        return false;
    }
}

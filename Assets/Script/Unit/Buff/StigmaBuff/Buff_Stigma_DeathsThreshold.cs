using UnityEngine;

public class Buff_Stigma_DeathsThreshold : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_DeathsThreshold;

        _name = "Death's Threshold";

        _description = "Death's Threshold Info";

        _buffActiveTiming = ActiveTiming.TURN_END | ActiveTiming.AFTER_ATTACKED | ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_owner.GetHP() <= 15 && !IsActive)
        {
            _owner.SetBuff(new Buff_SacredStep());
            IsActive = true;
        }

        return false;
    }
}
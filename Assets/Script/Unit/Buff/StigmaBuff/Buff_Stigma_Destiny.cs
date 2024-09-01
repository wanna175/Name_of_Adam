using UnityEngine;

public class Buff_Stigma_Destiny : Buff
{    
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Destiny;

        _name = "Destiny";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (RandomManager.GetFlag(0.6f))
        {
            _owner.ChangedDamage += _owner.BattleUnitTotalStat.ATK;
        }
        else
        {
            _owner.ChangedDamage -= _owner.BattleUnitTotalStat.ATK / 2;
        }

        return false;
    }
}
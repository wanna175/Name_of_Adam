using UnityEngine;

public class Buff_Stigma_Dispel : Buff
{
    public override void Init( BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Dispel;

        _name = "Dispel";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.Buff.DispelBuff();

        return false;
    }
}
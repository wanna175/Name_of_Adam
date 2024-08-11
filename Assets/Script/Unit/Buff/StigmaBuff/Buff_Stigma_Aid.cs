using UnityEngine;

public class Buff_Stigma_Aid : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Aid;

        _name = "Aid";

        _buffActiveTiming = ActiveTiming.AFTER_SWITCH;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        caster.GetHeal(15, _owner);

        return false;
    }
}
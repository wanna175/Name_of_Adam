using UnityEngine;

public class Buff_Stigma_Purification : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Purification;

        _name = "Purification";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && _owner.Buff.GetHasBuffNum() >= 2)
            caster.ChangeFall(1, _owner, FallAnimMode.On);

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Intercession : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Intercession;

        _name = "Intercession";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && _owner.AttackUnitNum > 1)
            caster.ChangeFall(1, _owner, FallAnimMode.On);

        return false;
    }
}

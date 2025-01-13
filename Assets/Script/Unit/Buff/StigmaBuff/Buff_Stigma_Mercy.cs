using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Mercy : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Mercy;

        _name = "Mercy";

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && caster.BattleUnitTotalStat.FallMaxCount / 2 >= caster.BattleUnitTotalStat.FallMaxCount - caster.Fall.GetCurrentFallCount())
            caster.ChangeFall(1, _owner, FallAnimMode.On);

        return false;
    }
}

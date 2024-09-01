using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Dusk : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Dusk;

        _name = "Dusk";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        int count = caster.Buff.GetBuffStack(BuffEnum.Dusk);
        
        if (count >= 1)
        {
            List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);

            foreach (BattleUnit unit in targetUnits)
            {
                Debug.Log(unit);
                if (unit.Team == caster.Team)
                    unit.GetAttack(-_owner.BattleUnitTotalStat.ATK, null);
            }
        }
        if (count >= 2)
        {
            caster.ChangeFall(1, FallAnimMode.On);
        }

        Buff_Dusk dusk = new();
        caster.SetBuff(dusk);

        return false;
    }
}
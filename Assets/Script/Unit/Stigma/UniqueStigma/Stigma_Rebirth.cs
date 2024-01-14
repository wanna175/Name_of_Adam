using UnityEngine;
using System.Collections.Generic;

public class Stigma_Rebirth : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_Rebirth());
        caster.SetBuff(new Buff_Vice());

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location, caster.Team == Team.Player ? Team.Enemy : Team.Player);
        foreach (BattleUnit unit in targetUnits)
        {
            unit.GetAttack(-10, caster);
        }
    }
}
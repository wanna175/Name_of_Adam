using System.Collections.Generic;
using UnityEngine;

public class Stigma_Raise : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location, caster.Team);

        caster.SetBuff(new Buff_Raise());
        if (name.Contains("II"))
            caster.SetBuff(new Buff_Raise());

        foreach (BattleUnit unit in targetUnits)
        {
            unit.SetBuff(new Buff_Raise());
            if (name.Contains("II"))
                unit.SetBuff(new Buff_Raise());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class 가학 : Passive
{
    private bool isApplied = false;

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (isApplied)
            return;

        caster.BattleUnitChangedStat.ATK += 3;
        isApplied = true;
    }
}
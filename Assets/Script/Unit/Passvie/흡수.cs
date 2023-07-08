using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class 흡수 : Passive
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        base.Use(caster, receiver);

        double heal = caster.BattleUnitTotalStat.ATK * 0.3;
        caster.ChangeHP(((int)heal));
    }
}
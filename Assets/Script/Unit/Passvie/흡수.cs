using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class 흡수 : Passive
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        double heal = caster.Stat.ATK * 0.3;
        caster.ChangeHP(((int)heal));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class 대죄 : Passive
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        receiver.ChangeFall(1);
    }
}

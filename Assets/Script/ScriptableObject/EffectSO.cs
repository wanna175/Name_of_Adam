using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public virtual void Effect(BattleUnit caster) { }

    public virtual void Effect(BattleUnit caster, List<BattleUnit> battleUnits) { }
}

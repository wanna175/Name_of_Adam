using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void Effect(BattleUnit ch);

    public virtual List<Vector2> GetRange() => null;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public abstract void Effect(Character ch);

    public virtual List<Vector2> GetRange() => null;
}

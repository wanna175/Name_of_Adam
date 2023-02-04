using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Stat
{
    public float HP;
    public float ATK;
    public int SPD;
}

[Serializable]
public enum RangeType
{
    Melee,
    Ranged
}

[Serializable]
public enum Team
{
    Player,
    Enemy,
}

using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Stat
{
    public int HP;
    public int CurrentHP;
    public int ATK;
    public int SPD;
    public int FallCurrentCount;
    public int FallMaxCount;
    public int ManaCost;

    public static Stat operator +(Stat lhs, Stat rhs)
    {
        Stat result = new Stat();
        result.HP = lhs.HP + rhs.HP;
        result.CurrentHP = lhs.CurrentHP + rhs.CurrentHP;
        result.ATK = lhs.ATK + rhs.ATK;
        result.SPD = lhs.SPD + rhs.SPD;
        result.FallCurrentCount = lhs.FallCurrentCount + rhs.FallCurrentCount;
        result.FallMaxCount = lhs.FallMaxCount + rhs.FallMaxCount;
        result.ManaCost = lhs.ManaCost + rhs.ManaCost;

        return result;
    }

    public static Stat operator -(Stat lhs, Stat rhs)
    {
        Stat result = new Stat();
        result.HP = lhs.HP - rhs.HP;
        result.CurrentHP = lhs.CurrentHP - rhs.CurrentHP;
        result.ATK = lhs.ATK - rhs.ATK;
        result.SPD = lhs.SPD - rhs.SPD;
        result.FallCurrentCount = lhs.FallCurrentCount - rhs.FallCurrentCount;
        result.FallMaxCount = lhs.FallMaxCount - rhs.FallMaxCount;
        result.ManaCost = lhs.ManaCost - rhs.ManaCost;

        return result;
    }
}

[Serializable]
public enum Team
{
    Player,
    Enemy,
}

[SerializeField]
public enum Faction
{
    순리의_기사단,
    짐승,
    바벨,
}

[SerializeField]
public enum BehaviorType
{
    원거리,
    근거리,
    서포터,
    탱커,
    전사,
    시즈,
    칼로스,
}

[SerializeField]
public enum Rarity
{
    레어,
    엘리트,
}

[SerializeField]
public enum TargetType
{
    Select,
    Range,
}

public enum ClickType
{
    Move = 0,
    Attack = 1,
}

[SerializeField]
public enum Scene
{
    Battle,
}

[Serializable]
public struct TestUnit
{
    public GameObject Unit;
    public Vector2 Location;
    public Team Team;
}


public enum Sound
{
    Bgm,
    Effect,
    MaxCount,
}

public enum PassiveType 
{ 
    BEFOREATTACK, 
    AFTERATTACK, 
    BEFOREATTACKED, 
    AFTERATTACKED, 
    FALL, 
    FALLED, 
    MOVE, 
    SUMMON, 
    ETC 
};


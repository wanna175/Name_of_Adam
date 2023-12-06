using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Stat
{
    public int MaxHP;
    public int CurrentHP;
    public int ATK;
    public int SPD;
    public int FallCurrentCount;
    public int FallMaxCount;
    public int ManaCost;

    public static Stat operator +(Stat lhs, Stat rhs)
    {
        Stat result = new();
        result.MaxHP = lhs.MaxHP + rhs.MaxHP;
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
        Stat result = new();
        result.MaxHP = lhs.MaxHP - rhs.MaxHP;
        result.CurrentHP = lhs.CurrentHP - rhs.CurrentHP;
        result.ATK = lhs.ATK - rhs.ATK;
        result.SPD = lhs.SPD - rhs.SPD;
        result.FallCurrentCount = lhs.FallCurrentCount - rhs.FallCurrentCount;
        result.FallMaxCount = lhs.FallMaxCount - rhs.FallMaxCount;
        result.ManaCost = lhs.ManaCost - rhs.ManaCost;

        return result;
    }

    public void ClearStat()
    {
        MaxHP = 0;
        CurrentHP = 0;
        ATK = 0;
        SPD = 0;
        FallCurrentCount = 0;
        FallMaxCount = 0;
        ManaCost = 0;
    }
}

[Serializable]
public enum Team
{
    Player,
    Enemy,
}

public enum StageName
{
    none,

    StigmaStore = 1,
    UpgradeStore = 2,
    MoneyStore,
    Harlot,
    RandomEvent,
    CommonBattle,
    EliteBattle,
    BossBattle,


    Random
}

[SerializeField]
public enum Rarity
{
    일반,
    엘리트,
    오리지널,
}

[SerializeField]
public enum CutSceneMoveType
{
    stand,
    tracking
}

public enum Sounds
{
    BGM,
    Effect,
    MaxCount,
}

[Flags]
public enum ActiveTiming
{
    STIGMA = 1 << 1, //낙인 발동(소환 시, 낙인 부여 시)

    FIELD_UNIT_SUMMON = 1 << 2,//필드에 유닛이 소환 시
    SUMMON = 1 << 3, //소환 후

    TURN_START = 1 << 4, //턴 시작 시
    TURN_END = 1 << 5, //턴 종료 시

    ACTION_TURN_START = 1 << 6, //이동턴 전, 공격턴 전 통합

    MOVE_TURN_START = 1 << 7, //이동턴 전

    MOVE = 1 << 8, //이동 후

    MOVE_TURN_END = 1 << 9, //이동턴 후

    ATTACK_TURN_START = 1 << 10, //공격턴 전

    BEFORE_ATTACK = 1 << 11, //공격 전
    AFTER_ATTACK = 1 << 12, //공격 후

    DAMAGE_CONFIRM = 1 << 13, //대미지 확정

    BEFORE_ATTACKED = 1 << 14, //피격 전
    AFTER_ATTACKED = 1 << 15, //피격 후

    ATTACK_TURN_END = 1 << 16, //공격턴 후

    FALL = 1 << 17, //타락시켰을 때, 그 후
    FALLED = 1 << 18, //타락되었을 때 그 전

    BEFORE_UNIT_DEAD = 1 << 19, //자신이 사망 전
    AFTER_UNIT_DEAD = 1 << 20, //자신이 사망 후
    FIELD_UNIT_DEAD = 1 << 21, //필드 유닛이 사망 시

    UNIT_KILL = 1 << 22, //다른 유닛을 죽일 시
    UNIT_TERMINATE = 1 << 23, //다른 유닛을 제거 시(타락시켰을 때, 죽였을 때)

    NONE = 1 << 24 //없음
};

public enum StigmaTier
{
    Tier1,
    Tier2,
    Tier3,
    Unique,
    Harlot
}

public enum FieldColorType
{
    none,
    UnitSpawn,
    Move,
    Attack,
    Select,
    PlayerSkill,
    UltimatePlayerSkill
}

public enum PlayerSkillTargetType
{
    none,
    all,
    Unit,
    Enemy,
    Friendly
}

public enum BuffEnum
{
    Hook,
    Dispel,
    ShadowStep,
    Blessing,
    Sadism,
    Benediction,
    Sin,
    Regeneration,
    Gamble,
    BloodBlessing,
    BloodFest,
    Repetance,
    Martyrdom,
    Misdeed,
    Expand,
    BishopsPraise,
    Thirst,
    SolarEclipse,
    LunarEclipse,

    DeathStrike,

    Raise,
    Invincible,
    Immortal,
    Tailwind,

    Vice,
    TraceOfSolar,
    TraceOfLunar,

    Stun,

    InevitableEnd,

    Assasination,
    Berserker,
    PrayInAid,
    Karma,
    ForbiddenPact,
    Teleport,
}

public enum UnitActionType
{
    UnitAction,
    UnitAction_None,

    UnitAction_Iana,
    UnitAction_Nimrod,
    UnitAction_Trinity,
    UnitAction_Centaurus
}

public enum EffectTileType
{
    None,
    Nimrod_Attack_Enemy,
    Nimrod_Attack_Friendly
}
using System;
using System.Collections;
using System.Collections.Generic;
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
    public int CurrentUpgradeCount;

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
        result.CurrentUpgradeCount = lhs.CurrentUpgradeCount + rhs.CurrentUpgradeCount;

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
        result.CurrentUpgradeCount = lhs.CurrentUpgradeCount - rhs.CurrentUpgradeCount;

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
        CurrentUpgradeCount = 0;
    }
}

public struct Upgrade
{
    public string UpgradeName;
    public string UpgradeDescription;
    public Sprite UpgradeImage;
    public Sprite UpgradeDecorationImage;
    public Stat UpgradeStat;
    public UpgradeData UpgradeData;
}

[Serializable]
public class UpgradeData
{
    public string ID;
    public int Rarity;
    public string Name;
    public string Description;
    public string Image;

    public string HP;
    public string ATK;
    public string SPD;
    public string COST;
}

[Serializable]
public class UpgradeLoader
{
    public List<UpgradeData> UpgradeData = new();
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
    Normal,
    Elite,
    Original,
    Boss
}

[SerializeField]
public enum CutSceneMoveType
{
    stand,
    tracking,
    noneMove
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
    FIELD_ATTACK_TURN_END = 1 << 17, //필드 유닛의 공격턴 후

    FALL = 1 << 18, //타락시켰을 때, 그 후
    FALLED = 1 << 19, //타락되었을 때 그 전
    FIELD_UNIT_FALLED = 1 << 20, //필드 유닛이 타락 시

    BEFORE_UNIT_DEAD = 1 << 21, //자신이 사망 전
    AFTER_UNIT_DEAD = 1 << 22, //자신이 사망 후
    FIELD_UNIT_DEAD = 1 << 23, //필드 유닛이 사망 시

    UNIT_KILL = 1 << 24, //다른 유닛을 죽일 시
    UNIT_TERMINATE = 1 << 25, //다른 유닛을 제거 시(타락시켰을 때, 죽였을 때)

    ATTACK_MOTION_END = 1 << 26, //공격 모션이 끝난 뒤

    NONE = 1 << 27 //없음
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

public enum StigmaEnum
{
    Absorption,
    Additional_Punishment,
    Assasination,
    Benevolence,
    Berserker,
    BishopsPraise,
    Blessing,
    BloodBlessing,
    Cleanse,
    DeathStrike,
    Dispel,
    Expand,
    ForbiddenPact,
    Gamble,
    Hook,
    Immortal,
    Invincible,
    Karma,
    Killing_Spree,
    Martyrdom,
    Mercy,
    Misdeed,
    Pray_In_Aid,
    Raise,
    Regeneration,
    Repetance,
    Sadism,
    Shadow_Step,
    Sin,
    Tail_Wind,
    Teleport,
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
    TraceOfDust,
    Dust,

    DeathStrike,

    Raise,
    Invincible,
    Immortal,
    Tailwind,

    Vice,
    TraceOfSolar,
    TraceOfLunar,

    Stun,

    AfterMotionTransparent,
    AfterAttackDead,
    AfterAttackBounce,
    MarkOfRaquel,

    Assasination,
    Berserker,
    PrayInAid,
    Karma,
    ForbiddenPact,
    Teleport,
    KillingSpree,

    Collapse,
    PermanenceOfBabel,
    LegacyOfBabel,
    WrathOfBabel,
    Charge,
    Rebirth,
    Trinity,

    Leah,
    Raquel,
}

public enum UnitActionType
{
    UnitAction,
    UnitAction_None,

    UnitAction_Iana,
    UnitAction_Phanuel,
    UnitAction_Appaim,
    UnitAction_Tubalcain,
    UnitAction_Horus,
    UnitAction_Horus_Egg,
    UnitAction_Laser,
    UnitAction_RaquelLeah,
    UnitAction_CenteredSplash
}

public enum UnitMoveType
{
    UnitMove,
    UnitMove_None
}

public enum EffectTileType
{
    None,
    Phanuel_Attack_Enemy,
    Phanuel_Attack_Friendly
}

public enum TutorialStep
{
    UI_PlayerTurn = TutorialManager.STEP_BOUNDARY,
    UI_UnitTurn = UI_PlayerTurn + TutorialManager.STEP_BOUNDARY,

    Tooltip_PlayerTurn,
    Tooltip_ManaInfo,
    Tooltip_UnitInfo,
    Tooltip_PlayerSkillInfo,
    Tooltip_DeckUnitSelect,
    Tooltip_UnitSpawnSelect,
    Tooltip_TurnEnd,
    Tooltip_SpeedTable,
    Tooltip_UnitMove,
    Tooltip_UnitAttack,
    Tutorial_End_1,

    UI_FallSystem = UI_UnitTurn + TutorialManager.STEP_BOUNDARY,
    UI_DarkEssenceInfo = UI_FallSystem + TutorialManager.STEP_BOUNDARY,
    UI_Stigma_1 = UI_DarkEssenceInfo + TutorialManager.STEP_BOUNDARY,
    UI_Stigma_2 = UI_Stigma_1 + TutorialManager.STEP_BOUNDARY,

    Tooltip_DarkEssenceInfo = UI_Stigma_2 + Tutorial_End_1 % TutorialManager.STEP_BOUNDARY,
    Tooltip_BlackKnightDeck,
    Tooltip_BlackKnightSpawn,
    Tooltip_BuffInfo,
    Tooltip_TurnEnd_2,
    Tooltip_TurnEnd_3,
    Tooltip_UnitAttack_2,
    Tooltip_PlayerSkillDeck,
    Tooltip_PlayerSkillUse,
    Tooltip_FallSelect,
    Tooltip_TurnEnd_4,
    Tooltip_UnitSwap,
    Tooltip_UnitAttack_3,
    Tooltip_UnitSwap_2,
    Tooltip_UnitAttack_4,
    Tutorial_End_2,

    UI_UnitDead = UI_Stigma_2 + TutorialManager.STEP_BOUNDARY,
    UI_Defeat = UI_UnitDead + TutorialManager.STEP_BOUNDARY,
    UI_Last = UI_Defeat + TutorialManager.STEP_BOUNDARY,

    Tutorial_End_3 = UI_Last + Tutorial_End_2 % TutorialManager.STEP_BOUNDARY,
}

public struct TooltipData
{
    public TutorialStep Step;
    public string Info;
    public int IndexToTooltip;
    public bool IsCtrl;
    public bool IsEnd;
}
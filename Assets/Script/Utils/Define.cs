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

    public bool Compare(Stat other)
    {
        if (MaxHP != other.MaxHP)
            return false;
        if (CurrentHP != other.CurrentHP)
            return false;
        if (ATK != other.ATK)
            return false;
        if (SPD != other.SPD)
            return false;
        if (FallCurrentCount != other.FallCurrentCount)
            return false;
        if (FallMaxCount != other.FallMaxCount)
            return false;
        if (ManaCost != other.ManaCost)
            return false;
        return true;
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

public struct RewardUnit
{
    public string PrivateKey;
    public string Name;
    public int PreFall;
    public Sprite Image;

    public RewardUnit(string privateKey, string name, int preFall, Sprite image)
    {
        PrivateKey = privateKey;
        Name = name;
        PreFall = preFall;
        Image = image;
    }
}

public struct Upgrade
{
    public string UpgradeName;
    public string UpgradeDescription;
    public Sprite UpgradeImage88;
    public Sprite UpgradeImage160;
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
public class StigmaSaveData
{
    public StigmaEnum StigmaEnum;
    public StigmaTier Tier;
}

[Serializable]
public class MapDataSaveData
{
    public string MapObject;           // 현재 맵 구조의 프리팹
    public List<StageData> StageList;// 맵의 노드들의 리스트
    public int CurrentTileID;            // 현재 위치하고있는 타일의 ID
    public List<int> ClearTileID;       // 이미 클리어한 타일의 ID
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

    Stigmata = 1,
    Baptism = 2,
    MoneyStore,
    Sacrifice,
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
    STIGMA = 1 << 1, //성흔 발동(소환 시, 성흔 부여 시)

    FIELD_UNIT_SUMMON = 1 << 2,//필드에 유닛이 소환 시
    SUMMON = 1 << 3, //소환 후

    TURN_START = 1 << 4, //턴 시작 시
    TURN_END = 1 << 5, //턴 종료 시

    UNIT_TURN_START = 1 << 6, //유닛턴 전

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

    BEFORE_CHANGE_FALL = 1 << 18, //타락시켰을 때, 그 후

    FALL = 1 << 19, //타락시켰을 때, 그 후
    FALLED = 1 << 20, //타락되었을 때 그 전
    FIELD_UNIT_FALLED = 1 << 21, //필드 유닛이 타락 시

    BEFORE_UNIT_DEAD = 1 << 22, //자신이 사망 전
    AFTER_UNIT_DEAD = 1 << 23, //자신이 사망 후
    FIELD_UNIT_DEAD = 1 << 24, //필드 유닛이 사망 시

    UNIT_KILL = 1 << 25, //다른 유닛을 죽일 시

    ATTACK_MOTION_END = 1 << 26, //공격 모션이 끝난 뒤
    AFTER_SWITCH = 1 << 27, //유닛 간 스위치 후
    BEFORE_BUFFED = 1 << 28, //버프를 얻기 전

    NONE = 1 << 29 //없음
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
    EnemyPlayerSkill,
    PlayerPlayerSkill
}

public enum PlayerSkillTargetType
{
    none,
    all,
    Unit,
    Enemy,
    Friendly,
    NotBattleOnly
}

public enum StigmaEnum
{
    //normal stigmata
    Absorption,
    AdditionalPunishment,
    Assasination,
    Benevolence,
    Berserker,
    Bishops_Praise,
    Blessing,
    BloodBlessing,
    Cleanse,
    DeathStrike,
    Destiny,
    Dispel,
    Expand,
    ForbiddenPact,
    Hook,
    Immortality,
    Invincibility,
    Killing_Spree,
    Martyrdom,
    Mercy,
    PrayInAid,
    Raise,
    Regeneration,
    Repetance,
    Sadism,
    ShadowStep,
    Sin,
    Tailwind,
    Teleport,
    ShadowCloak,
    Rearmament,
    Solitude,
    Grudge,
    HandOfGrace,
    Aid,
    DeathsThreshold,
    Fortification,
    ArmorOfAeons,
    BrokenSword,
    BloodOath,
    Gluttony,
    BlindFaith,

    //Unique stigmata 
    Birth = 100,
    Blooming = 101,
    Charge = 102,
    Collapse = 103,
    Dusk = 104,
    Funeral = 105,
    Karma = 106,
    LegacyOfBabel = 107,
    Advent = 108,
    Misdeed = 109,
    PermanenceOfBabel = 110,
    Rebirth = 111,
    Sacrifice = 112,
    Symbiosis = 113,
    Trinity = 114,
    WrathOfBabel = 115,
    Glory = 116,
    ThornsOfOblivion = 117,
    GardenOfOblivion = 118,
    StormSurge = 119,
    StormSurge2 = 120,
    StormSurge3 = 121,
    DeepSea = 122,
}

public enum BuffEnum
{
    //normal buff (image)
    None,

    Appaim,
    Divine,
    Berserker,
    Curse,
    DeathStrike,
    Edified,
    Immortality,
    Invincibility,
    Karma,
    Lea,
    MarkOfBeast,
    Rahel,
    Sin,
    Stun,
    SpeedIncrease,
    Dusk,
    Malevolence,
    Grudge,
    SacredStep,
    AttackDecrease,
    AttackBoost,
    KillingSpree,
    Libiel,
    EliteStatBuff,

    //systemic buff (no image)
    AfterAttackBounce,
    AfterAttackDead,
    AfterMotionTransparent,
    StatBuff,

    //stigmata buff (no image)
    Stigmata_Absorption,
    Stigmata_AdditionalPunishment,
    Stigmata_Assasination,
    Stigmata_BishopsPraise,
    Stigmata_Blessing,
    Stigmata_BloodBlessing,
    Stigmata_Cleanse,
    Stigmata_Destiny,
    Stigmata_Dispel,
    Stigmata_Expand,
    Stigmata_ForbiddenPact,
    Stigmata_Hook,
    Stigmata_KillingSpree,
    Stigmata_Martyrdom,
    Stigmata_Mercy,
    Stigmata_Misdeed,
    Stigmata_PrayInAid,
    Stigmata_Regeneration,
    Stigmata_Repetance,
    Stigmata_Sadism,
    Stigmata_ShadowStep,
    Stigmata_Rearmament,
    Stigmata_Solitude,
    Stigmata_Grudge,
    Stigmata_Aid,
    Stigmata_ShadowCloak,
    Stigmata_HandOfGrace,
    Stigmata_Teleport,
    Stigmata_DeathsThreshold,
    Stigmata_Fortification,
    Stigmata_ArmorOfAeons,
    Stigmata_BrokenSword,
    Stigmata_BloodOath,
    Stigmata_Gluttony,
    Stigmata_BlindFaith,

    //unique stigma buff (no image)
    Stigmata_Birth,
    Stigmata_Blooming,
    Stigmata_Charge,
    Stigmata_Collapse,
    Stigmata_Dusk,
    Stigmata_LegacyOfBabel,
    Stigmata_Advent,
    Stigmata_PermanenceOfBabel,
    Stigmata_Rebirth,
    Stigmata_Sacrifice,
    Stigmata_Symbiosis,
    Stigmata_Trinity,
    Stigmata_WrathOfBabel,
    Stigmata_Glory,
    Stigmata_ThornsOfOblivion,
    Stigmata_GardenOfOblivion,
    Stigmata_StormSurge,
    Stigmata_StormSurge2,
    Stigmata_StormSurge3,
    Stigmata_DeepSea,

    Stigmata_BloodFest,
    Stigmata_Thirst,
    Stigmata_TraceOfSolar,
    Stigmata_TraceOfLunar,
}

public enum UnitActionType
{
    UnitAction,
    UnitAction_None,

    UnitAction_Iana,
    UnitAction_Phanuel,
    UnitAction_Appaim,
    UnitAction_Tubalcain,
    UnitAction_Savior,
    UnitAction_FlowerOfSacrifice,
    UnitAction_Laser,
    UnitAction_RahelLea,
    UnitAction_CenteredSplash,
    UnitAction_Libiel,
    UnitAction_Arabella,
    UnitAction_Yohrn,
    UnitAction_Yohrn_Body,
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

    UI_Divine = UI_Stigma_2 + TutorialManager.STEP_BOUNDARY,
    UI_Defeat = UI_Divine + TutorialManager.STEP_BOUNDARY,
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

public enum CutSceneType
{
    Main,
    Tutorial,

    // 엘리트
    Elieus_Enter,
    RahelLea_Enter,
    Appaim_Enter,
    Libiel_Enter,
    Arabella_Enter,

    // 보스
    Phanuel_Enter,
    Phanuel_Dead,
    TheSavior_Enter,
    TheSavior_Dead,
    Yohrn_Enter,
    Yohrn_Dead,

    // NPC
    NPC_Baptism_Corrupt,
    NPC_Stigmata_Corrupt,
    NPC_Sacrifice_Corrupt,
}

public enum FallAnimMode
{
    On,
    Off
}

public enum CurrentEvent
{
    None,
    Upgrade_Select,
    Upgrade_Full_Exception,
    Heal_Faith_Select,
    Stigmata_Select,
    Stigmata_Give,
    Stigmata_Receive,
    Stigmata_Full_Exception,
    Revert_Unit_Select,
    Corrupt_Stigmata_Select,

    Complete_Upgrade,
    Complete_Heal_Faith,
    Complate_Stigmata,
    Complate_Apostle,

    Hall_Delete,
};

[Serializable]
public class Script
{
    public string name;
    public string script;
}

[Serializable]
public class Content
{
    public string title;
    public List<Script> contents = new List<Script>();
}

[Serializable]
public class StageList
{
    public int Level;
    public List<StageSpawnData> StageData;
}

[Serializable]
public class StageSpawnData
{
    public int ID;
    public List<StageUnitData> Units;
}

[Serializable]
public class StageUnitData
{
    public string Name;
    public Vector2 Location;
}


[Serializable]
public class ScriptLoader : ILoader<string, List<Script>>
{
    public List<Content> scripts = new List<Content>();

    public Dictionary<string, List<Script>> MakeDict()
    {
        Dictionary<string, List<Script>> dic = new Dictionary<string, List<Script>>();
        foreach (Content content in scripts)
        {
            dic.Add(content.title, content.contents);
        }
        return dic;
    }

}
[Serializable]
public class StageLoader : ILoader<int, List<StageSpawnData>>
{
    public List<StageList> StageList = new List<StageList>();

    public Dictionary<int, List<StageSpawnData>> MakeDict()
    {
        Dictionary<int, List<StageSpawnData>> dic = new Dictionary<int, List<StageSpawnData>>();
        foreach (StageList stage in StageList)
        {
            dic.Add(stage.Level, stage.StageData);
        }
        return dic;
    }
}

public class ProgressLoader : ILoader<int, ProgressItem>
{
    public List<ProgressItem> ProgressItems = new List<ProgressItem>();

    public Dictionary<int, ProgressItem> MakeDict()
    {
        Dictionary<int, ProgressItem> dic = new Dictionary<int, ProgressItem>();
        foreach (ProgressItem progressItem in ProgressItems)
        {
            dic.Add(progressItem.ID, progressItem);
        }
        return dic;
    }
}

public enum SortMode
{
    Default,    // 계급 (성흔 보유 수에 따라 내부 정렬)
    Attack,
    HP,
    Speed,
    Cost,
    Hall,       // 전당 유닛 + 나머지 유닛 (성흔 보유 수에 따라 정렬)
}
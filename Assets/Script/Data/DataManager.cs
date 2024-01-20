using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : MonoBehaviour
{
    // public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    public Dictionary<int, List<StageSpawnData>> StageDatas = new Dictionary<int, List<StageSpawnData>>();
    public MapData Map;
    public int StageAct; // 현재 맵이 몇 막인지 기록하는 변수. 0 : 튜토리얼, 1 : 1막, 2 : 2막 이런식으로

    [SerializeField] public GameData GameData;
    [SerializeField] public GameData GameDataMain;
    [SerializeField] public GameData GameDataTutorial;

    [SerializeField] public GameData GameDataMainLayout; //디버깅용 데이터

    public Dictionary<string, List<Script>> ScriptData = new Dictionary<string, List<Script>>();
    public StigmaController StigmaController;

    public void Init()
    {
        // StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
        StageDatas = LoadJson<StageLoader, int, List<StageSpawnData>>("StageData").MakeDict();
        ScriptData = LoadJson<ScriptLoader, string, List<Script>>("Script").MakeDict();
        StigmaController = new StigmaController();
        Map = new MapData();

        if (GameManager.SaveManager.SaveFileCheck())
            GameManager.SaveManager.LoadGame();

        _darkEssense = GameData.DarkEssence;
        if(GameData.Incarna != null)
        {
            _playerSkillCount = GameData.Incarna.PlayerSkillCount;
        }
        else
        {
            _playerSkillCount = 0;
        }
    }

    public void MainDeckSet()
    {
        GameData.Incarna = GameDataMain.Incarna;
        GameData.Money = GameDataMain.Money;
        GameData.DarkEssence = GameDataMain.DarkEssence;
        GameData.PlayerHP = GameDataMain.PlayerHP;
        GameData.PlayerSkillCount = GameDataMain.PlayerSkillCount;
        GameData.DeckUnits = GameDataMain.DeckUnits;
        GameData.FallenUnits = GameDataMain.FallenUnits;
        GameData.isVisitUpgrade = GameDataMain.isVisitUpgrade;
        GameData.isVisitStigma = GameDataMain.isVisitStigma;
        GameData.Progress.ClearProgress();
        _darkEssense = GameData.DarkEssence;
        Debug.Log("GameData.DeckUnit: " + GameData.DeckUnits.Count);
        //OutGame에서 업그레이드 된 스탯 + 낙인 불러와야해서 ClearStat 사용하면 안됨, 파생되는 문제 발생 시 수정 필요 
        /*
        foreach (DeckUnit unit in GameData.DeckUnits)
        {
            unit.DeckUnitChangedStat.ClearStat();
            unit.DeckUnitUpgradeStat.ClearStat();
            unit.ClearStigma();
        }
        */
    }

    public void DeckClear()
    {
        GameData.Incarna = GameDataTutorial.Incarna;
        GameData.Money = GameDataTutorial.Money;
        GameData.DarkEssence = GameDataTutorial.DarkEssence;
        GameData.PlayerHP = GameDataTutorial.PlayerHP;
        GameData.PlayerSkillCount = GameDataTutorial.PlayerSkillCount;
        GameData.DeckUnits = GameDataTutorial.DeckUnits;
        GameData.FallenUnits.Clear();
        GameData.isVisitUpgrade = GameDataTutorial.isVisitUpgrade;
        GameData.isVisitStigma = GameDataTutorial.isVisitStigma;
        GameData.Progress.ClearProgress();

        foreach (DeckUnit unit in GameData.DeckUnits)
        {
            unit.DeckUnitChangedStat.ClearStat();
            unit.DeckUnitUpgradeStat.ClearStat();
            unit.ClearStigma();
        }
    }

    //디버깅용 메서드
    public void MainDeckLayoutSet()
    {
        GameDataMain.Incarna = GameDataMainLayout.Incarna;
        GameDataMain.Money = GameDataMainLayout.Money;
        GameDataMain.DarkEssence = GameDataMainLayout.DarkEssence;
        GameDataMain.PlayerHP = GameDataMainLayout.PlayerHP;
        GameDataMain.PlayerSkillCount = GameDataMainLayout.PlayerSkillCount;
        GameDataMain.DeckUnits = GameDataMainLayout.DeckUnits;
        GameDataMain.FallenUnits = GameDataMainLayout.FallenUnits;
        GameDataMain.isVisitUpgrade = GameDataMainLayout.isVisitUpgrade;
        GameDataMain.isVisitStigma = GameDataMainLayout.isVisitStigma;
        GameDataMain.Progress.ClearProgress();

        foreach (DeckUnit unit in GameDataMain.DeckUnits)
        {
            unit.DeckUnitChangedStat.ClearStat();
            unit.DeckUnitUpgradeStat.ClearStat();
            unit.ClearStigma();
        }
    }

    public void HallSelectedDeckSet()
    {
        List<DeckUnit> EliteHallHandDeck = new();
        List<DeckUnit> NormalHallHandDeck = new();

        foreach (DeckUnit unit in GameData.DeckUnits)
        {
            if (unit.IsMainDeck)
            {
                if(unit.Data.Rarity != Rarity.Normal)
                {
                    EliteHallHandDeck.Add(unit);
                    Debug.Log(unit.Data.Name);
                }
                else
                {
                    NormalHallHandDeck.Add(unit);
                }
            }
        }

        EliteHallHandDeck.AddRange(NormalHallHandDeck);

        GameDataMain.DeckUnits = EliteHallHandDeck;
    }

    public void HallDeckSet()
    {
        GameData.DeckUnits = GameManager.OutGameData.SetHallDeck();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    T LoadJson<T>(string path)
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/{path}");

        return JsonUtility.FromJson<T>(textAsset.text);
    }

    [SerializeField] private List<DeckUnit> _playerDeck = new ();
    public List<DeckUnit> PlayerDeck => _playerDeck;

    public void AddDeckUnit(DeckUnit unit)
    {
        GameData.DeckUnits.Add(unit);
    }

    public void RemoveDeckUnit(DeckUnit unit)
    {
        GameData.DeckUnits.Remove(unit);
    }

    public List<DeckUnit> GetDeck()
    {
        return GameData.DeckUnits;
    }

    public void SetDeck(List<DeckUnit> deck)
    {
        GameData.DeckUnits = deck;
    }

    private int _money;
    public int Money => _money;

    public bool MoneyChage(int cost)
    {
        if (_money + cost < 0)
        {
            return false;
        }
        else
        {
            _money += cost;
            return true;
        }
    }

    private int _darkEssense = 0;
    public int DarkEssense => _darkEssense;

    public bool DarkEssenseChage(int cost)
    {
        Debug.Log("Dark Essense: " + _darkEssense + " Change: " + cost);
        if (_darkEssense + cost < 0)
        {
            return false;
        }
        else
        {
            _darkEssense += cost;
            return true;
        }
    }

    public bool CanUseDarkEssense(int value)
    {
        if (_darkEssense >= value)
            return true;

        Debug.Log("not enough Dark Essense");
        return false;
    }

    private int _playerSkillCount = 0;
    public int PlayerSkillCount => _playerSkillCount;

    public bool PlayerSkillCountChage(int cost)
    {
        Debug.Log("Player Skill Count: " + _playerSkillCount + " Change: " + cost);
        if (_playerSkillCount + cost < 0)
        {
            return false;
        }
        else
        {
            _playerSkillCount += cost;
            return true;
        }
    }

    public List<PlayerSkill> GetPlayerSkillList()
    {
        List<PlayerSkill> skillList = new ();

        foreach (PlayerSkill skill in GameData.Incarna.PlayerSkillList)
        {
            skillList.Add(skill);
        }

        //skillList.Insert(2, GameData.UniversalPlayerSkill); //Universal Skill 보류

        return skillList;
    }

    public List<int> GetProbability()
    {
        List<int> probability = new();
        probability.Add(99);
        probability.Add(89);

        return probability;
    }
}
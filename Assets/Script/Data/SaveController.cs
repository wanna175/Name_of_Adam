using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveData
{
    public MapDataSaveData MapData;
    public string IncarnaID;
    public List<SaveUnit> DeckUnitData;
    public List<SaveUnit> FallenUnitsData;
    public int DarkEssence;
    public int PlayerHP;
    public Progress ProgressData;
    public NPCQuest NpcQuestData;
    public int StageAct;
    public Vector3 StageBenediction;
}

[Serializable]
public class SaveUnit
{
    public string UnitDataID;
    public Stat UnitStat;
    public List<StigmaSaveData> Stigmata; // 유닛에게 추가된 낙인
    public List<UpgradeData> Upgrades; // 유닛에게 추가된 강화
}

public class SaveController : MonoBehaviour
{
    private const string encryptionKey = "EncryptSaveData!@#$%^&*()_+";
    private const string SaveDataFileName = "867755930112.dat";

    string path;

    public void Init()
    {
        // 사용자/Appdata//LocalLow에 자신의 파일이 있는지 확인하는 함수(System.IO 필요)
        path = Path.Combine(Application.persistentDataPath, SaveDataFileName);
    }

    // 게임 진행 정보 저장
    // 저장하는 데이터가 늘어나면 이곳에서 관리해주어야 함
    public void SaveGame()
    {
        List<SaveUnit> saveDeckUnitList = new();
        List<SaveUnit> saveFallenUnitList = new();
        SaveData newData = new();
        GameData CurGameData = GameManager.Data.GameData;

        foreach (DeckUnit unit in CurGameData.DeckUnits)
        {
            SaveUnit saveUnit = new();

            saveUnit.UnitDataID = unit.Data.ID;
            saveUnit.UnitStat = unit.DeckUnitUpgradeStat;
            saveUnit.Stigmata = unit.GetStigmaSaveData();
            saveUnit.Upgrades = unit.GetUpgradeData();

            saveDeckUnitList.Add(saveUnit);
        }

        foreach (DeckUnit unit in CurGameData.FallenUnits)
        {
            SaveUnit saveUnit = new();

            saveUnit.UnitDataID = unit.Data.ID;
            saveUnit.UnitStat = unit.DeckUnitUpgradeStat;
            saveUnit.Stigmata = unit.GetStigmaSaveData();
            saveUnit.Upgrades = unit.GetUpgradeData();

            saveFallenUnitList.Add(saveUnit);
        }

        newData.MapData = GameManager.Data.GetMapSaveData();
        newData.DeckUnitData = saveDeckUnitList;
        newData.FallenUnitsData = saveFallenUnitList;
        newData.IncarnaID = CurGameData.Incarna.ID;
        newData.DarkEssence = GameManager.Data.DarkEssense;
        newData.PlayerHP = CurGameData.PlayerHP;
        newData.ProgressData = CurGameData.Progress;
        newData.NpcQuestData = CurGameData.NpcQuest;
        newData.StageAct = GameManager.Data.StageAct;
        newData.StageBenediction = CurGameData.StageBenediction;

        GameManager.OutGameData.SetNPCQuest();
        string json = JsonUtility.ToJson(newData, true);

        File.WriteAllText(path, EncryptAndDecrypt(json));
    }

    // 게임 진행 정보 저장
    // 저장하는 데이터가 늘어나면 이곳에서 관리해주어야 함
    public void LoadGame()
    {
        string json = File.ReadAllText(path);
        SaveData loadData = JsonUtility.FromJson<SaveData>(EncryptAndDecrypt(json));

        GameManager.Data.SetMapSaveData(loadData.MapData);
        List<DeckUnit> savedDeckUnitList = new();
        List<DeckUnit> savedFallenUnitList = new();

        foreach (SaveUnit saveUnit in loadData.DeckUnitData)
        {
            DeckUnit deckunit = new();
            deckunit.Data = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{saveUnit.UnitDataID}");
            deckunit.DeckUnitUpgradeStat = saveUnit.UnitStat;
            deckunit.SetStigmaSaveData(saveUnit.Stigmata);
            deckunit.SetUpgrade(saveUnit.Upgrades);

            savedDeckUnitList.Add(deckunit);
        }

        foreach (SaveUnit saveUnit in loadData.FallenUnitsData)
        {
            DeckUnit deckunit = new();
            deckunit.Data = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{saveUnit.UnitDataID}");
            deckunit.DeckUnitUpgradeStat = saveUnit.UnitStat;
            deckunit.SetStigmaSaveData(saveUnit.Stigmata);
            deckunit.SetUpgrade(saveUnit.Upgrades);

            savedFallenUnitList.Add(deckunit);
        }

        GameManager.Data.GameData.DeckUnits = savedDeckUnitList;
        GameManager.Data.GameData.FallenUnits = savedFallenUnitList;
        GameManager.Data.GameData.Incarna = GameManager.Resource.Load<Incarna>($"ScriptableObject/Incarna/{loadData.IncarnaID}"); ;
        GameManager.Data.GameData.DarkEssence = loadData.DarkEssence;
        GameManager.Data.GameData.PlayerHP = loadData.PlayerHP;
        GameManager.Data.GameData.Progress = loadData.ProgressData;
        GameManager.Data.GameData.NpcQuest = loadData.NpcQuestData;
        GameManager.Data.StageAct = loadData.StageAct;
        GameManager.Data.GameData.StageBenediction = loadData.StageBenediction;
    }

    // 저장된 데이터가 있는지 확인
    public bool SaveFileCheck() => File.Exists(Path.Combine(Application.persistentDataPath, SaveDataFileName));

    // 저장된 데이터 삭제
    public void DeleteSaveData() => File.Delete(path);

    private string EncryptAndDecrypt(string data)
    {
        string result = string.Empty;

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }

        return result;
    }
}
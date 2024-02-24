﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveData
{
    public MapData MapData;
    public Incarna IncarnaData;
    public PlayerSkill UniversalPlayerSkillData;
    public List<SaveUnit> DeckUnitData;
    public List<SaveUnit> FallenUnitsData;
    public int DarkEssence;
    public int PlayerHP;
    public int PlayerSkillCount;
    public int DefaultMana;
    public int GuardCount;
    public Progress ProgressData;
    public NPCQuest NpcQuestData;
    public int StageAct;
    public Vector2 StageBenediction;
}

[Serializable]
public class SaveUnit
{
    public DeckUnit unit;
    public List<Stigma> Stigmata; // 유닛에게 추가된 낙인
    public List<UpgradeData> Upgrades; // 유닛에게 추가된 강화
}

public class SaveController : MonoBehaviour
{
    string path;

    public void Init()
    {
        // 사용자/Appdata//LocalLow에 자신의 파일이 있는지 확인하는 함수(System.IO 필요)
        path = Path.Combine(Application.persistentDataPath, "SaveData.json");
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

            saveUnit.unit = unit;
            saveUnit.Stigmata = unit.GetChangedStigma();
            saveUnit.Upgrades = unit.GetUpgradeData();

            saveDeckUnitList.Add(saveUnit);
        }

        foreach (DeckUnit unit in CurGameData.FallenUnits)
        {
            SaveUnit saveUnit = new();

            saveUnit.unit = unit;
            saveUnit.Stigmata = unit.GetChangedStigma();
            saveUnit.Upgrades = unit.GetUpgradeData();

            saveFallenUnitList.Add(saveUnit);
        }

        newData.MapData = GameManager.Data.Map;
        newData.DeckUnitData = saveDeckUnitList;
        newData.FallenUnitsData = saveFallenUnitList;
        newData.IncarnaData = CurGameData.Incarna;
        newData.DarkEssence = GameManager.Data.DarkEssense;
        newData.PlayerHP = CurGameData.PlayerHP;
        newData.DefaultMana = 50; // 진척도에 따라 바뀌는 인자들
        newData.GuardCount = 1;   // 이 인자들을 적용하는 곳이 생기면 연동하기
        newData.ProgressData = CurGameData.Progress;
        newData.NpcQuestData = CurGameData.NpcQuest;
        newData.StageAct = GameManager.Data.StageAct;
        newData.StageBenediction = CurGameData.StageBenediction;

        GameManager.OutGameData.setNPCQuest();
        string json = JsonUtility.ToJson(newData, true);

        File.WriteAllText(path, json);
    }

    // 게임 진행 정보 저장
    // 저장하는 데이터가 늘어나면 이곳에서 관리해주어야 함
    public void LoadGame()
    {
        string json = File.ReadAllText(path);
        SaveData loadData = JsonUtility.FromJson<SaveData>(json);

        GameManager.Data.Map = loadData.MapData;
        List<DeckUnit> savedDeckUnitList = new();
        List<DeckUnit> savedFallenUnitList = new();

        foreach (SaveUnit saveUnit in loadData.DeckUnitData)
        {
            DeckUnit deckunit = saveUnit.unit;
            deckunit.AddAllStigma(saveUnit.Stigmata);
            deckunit.SetUpgrade(saveUnit.Upgrades);

            savedDeckUnitList.Add(deckunit);
        }

        foreach (SaveUnit saveUnit in loadData.FallenUnitsData)
        {
            DeckUnit deckunit = saveUnit.unit;
            deckunit.AddAllStigma(saveUnit.Stigmata);
            deckunit.SetUpgrade(saveUnit.Upgrades);

            savedFallenUnitList.Add(deckunit);
        }

        GameManager.Data.GameData.DeckUnits = savedDeckUnitList;
        GameManager.Data.GameData.FallenUnits = savedFallenUnitList;
        GameManager.Data.GameData.Incarna = loadData.IncarnaData;
        GameManager.Data.GameData.DarkEssence = loadData.DarkEssence;
        GameManager.Data.GameData.PlayerHP = loadData.PlayerHP;
        GameManager.Data.GameData.Progress = loadData.ProgressData;
        GameManager.Data.GameData.NpcQuest = loadData.NpcQuestData;
        GameManager.Data.StageAct = loadData.StageAct;
        GameManager.Data.GameData.StageBenediction = loadData.StageBenediction;

        //CurGameData.DefaultMana
        //CurGameData.GuardCount
    }

    // 저장된 데이터가 있는지 확인
    public bool SaveFileCheck() => File.Exists(Path.Combine(Application.persistentDataPath, "SaveData.json"));

    // 저장된 데이터 삭제
    public void DeleteSaveData() => File.Delete(path);
}
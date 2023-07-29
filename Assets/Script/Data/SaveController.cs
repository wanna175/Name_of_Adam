using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveData
{
    public MapSaveData MapData;
    public Incarna IncarnaData;
    public PlayerSkill UniversalPlayerSkillData;
    public List<DeckUnit> DeckUnitData;
    public int DarkEssence;
    public int PlayerSkillCount;
    public int DefaultMana;
    public int GuardCount;
}

[Serializable]
public class MapSaveData
{
    // MapList(현재 맵 지도)
    public int CurrentTileID;
    // public int[] PassedTileID; // 지나온 타일(기획에 따라 다르게)
}

public class SaveController
{
    static string path
    {
        get {
            if (path == null)
                Path.Combine(Application.persistentDataPath, "SaveData.json");
            return path;
        }
        set { path = value; }
    }

    public static void SaveGame()
    {
        SaveData newData = new SaveData();
        GameData CurGameData = GameManager.Data.GameData;

        // Map

        newData.IncarnaData = CurGameData.Incarna;
        newData.DeckUnitData = CurGameData.DeckUnits;
        newData.UniversalPlayerSkillData = CurGameData.UniversalPlayerSkill;
        newData.DarkEssence = CurGameData.DarkEssence;
        newData.PlayerSkillCount = CurGameData.PlayerSkillCount;
        newData.DefaultMana = 50; // 
        newData.GuardCount = 1;   // 이 둘은 나중에 가져오기

        string json = JsonUtility.ToJson(newData, true);

        File.WriteAllText(path, json);
    }

    public static void LoadGame()
    {

        SaveData loadData = JsonUtility.FromJson<SaveData>(path);
        GameData CurGameData = GameManager.Data.GameData;

        // Map

        CurGameData.Incarna = loadData.IncarnaData;
        CurGameData.UniversalPlayerSkill = loadData.UniversalPlayerSkillData;
        CurGameData.DeckUnits = loadData.DeckUnitData;
        CurGameData.DarkEssence = loadData.DarkEssence;
        //CurGameData.DefaultMana
        //CurGameData.GuardCount
    }

    public static bool SaveFileCheck() => File.Exists(Path.Combine(Application.persistentDataPath, "SaveData.json")) ? true : false;

    public static void DeleteSaveData() => File.Delete(path);
}

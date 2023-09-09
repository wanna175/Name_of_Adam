using System;
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
    public List<DeckUnit> DeckUnitData;
    public int DarkEssence;
    public int PlayerSkillCount;
    public int DefaultMana;
    public int GuardCount;
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
        SaveData newData = new SaveData();
        GameData CurGameData = GameManager.Data.GameData;

        newData.MapData = GameManager.Data.Map;

        newData.IncarnaData = CurGameData.Incarna;
        newData.DeckUnitData = CurGameData.DeckUnits;
        newData.UniversalPlayerSkillData = CurGameData.UniversalPlayerSkill;
        newData.DarkEssence = CurGameData.DarkEssence;
        newData.PlayerSkillCount = CurGameData.PlayerSkillCount;
        newData.DefaultMana = 50; // 진척도에 따라 바뀌는 인자들
        newData.GuardCount = 1;   // 이 인자들을 적용하는 곳이 생기면 연동하기

        string json = JsonUtility.ToJson(newData, true);

        File.WriteAllText(path, json);
    }

    // 게임 진행 정보 저장
    // 저장하는 데이터가 늘어나면 이곳에서 관리해주어야 함
    public void LoadGame()
    {
        string json = File.ReadAllText(path);
        SaveData loadData = JsonUtility.FromJson<SaveData>(json);
        GameData CurGameData = GameManager.Data.GameData;

        GameManager.Data.Map = loadData.MapData;

        GameManager.Data.GameData.Incarna = loadData.IncarnaData;
        GameManager.Data.GameData.UniversalPlayerSkill = loadData.UniversalPlayerSkillData;
        GameManager.Data.GameData.DeckUnits = loadData.DeckUnitData;
        GameManager.Data.GameData.DarkEssence = loadData.DarkEssence;
        //CurGameData.DefaultMana
        //CurGameData.GuardCount
    }

    // 저장된 데이터가 있는지 확인
    public bool SaveFileCheck() => File.Exists(Path.Combine(Application.persistentDataPath, "SaveData.json"));

    // 저장된 데이터 삭제
    public void DeleteSaveData() => File.Delete(path);
}
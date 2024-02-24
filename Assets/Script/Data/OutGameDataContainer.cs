﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

//NPC타락퀘스트 & 진척도 & 전당 유닛 등의 인자를 저장 & 불러오는 기능

[Serializable]
public class OutGameData
{
    public int ProgressCoin;                 // 진척도 코인
    public List<ProgressItem> ProgressItems; // 진척도 상점의 상품들
    public List<HallUnit> HallUnit;          // 전당 유닛
    
    public bool TutorialClear = false;
    public bool PhanuelClear = false;
    public bool HorusClear = false;

    public bool isVisitUpgrade = false;
    public bool isVisitStigma = false;
    public bool isVisitDarkShop = false;
    public NPCQuest npcQuest;                //npc타락퀘스트
    public bool isGameOver = false;          //바로 전에 게임이 오버되었는지 체크
    public CutSceneData cutSceneData;        //현재 진행된 컷씬

    public int language;
    public int resolution;
    public bool isWindowed;
    public float masterSoundPower; // 0 ~ 1
    public float BGMSoundPower; // 0 ~ 1
    public float SESoundPower; // 0 ~ 1
}

[Serializable]
public class ProgressItem
{
    public int ID;             // 진척도 ID
    public string Name;        // 이름
    public int Cost;           // 가격
    public int Prequest;       // 선행 조건 ID
    public bool IsLock;        // 구매 가능한지 여부
    public bool IsUnlocked;    // 구매 완료 여부
    public string Description; // 설명
}


[Serializable]
public class HallUnit
{
    public int ID;                // 전당 내에서 식별을 위한 ID
    public string UnitName;       // 지금은 유닛 이름으로 받고있지만 ID로 받는 기능이 추가되면 변경해야함
    public Stat UpgradedStat;     //업그레이드된 스텟
    public bool IsMainDeck;       //유닛이 메인덱에 포함되었는지 유무 확인
    public List<Stigma> Stigmata; // 유닛에게 추가된 낙인
    public List<UpgradeData> Upgrades; //유닛에게 추가된 강화
}


public class OutGameDataContainer : MonoBehaviour
{
    // 현재 진행중인 게임에서 관리하는 아웃게임데이터
    OutGameData data;
    string path;
    List<Resolution> resolutions;

    public void Init()
    {
        // 사용자\AppData\localLow에 있는 SaveData.json의 경로
        path = Path.Combine(Application.persistentDataPath, "OutGameSaveData.json");

        resolutions = new List<Resolution>
        {
            GetResolution(1920, 1080, 144),
            GetResolution(1280, 720, 144),
            GetResolution(640, 480, 144)
        };

        LoadData();
        SetResolution();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        try
        {
            // AppData에 파일이 있으면 OutGameData파일이 있으면 불러오기
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<OutGameData>(json);
        }
        catch(FileNotFoundException e)
        {
            // 파일이 없을 경우(처음 플레이 시 or 데이터 삭제 시) 새로운 데이터 생성
            Debug.Log(e);
            CreateData();
        }
    }

    public List<DeckUnit> SetHallDeck()
    {
        List<DeckUnit> HallList = new();

        foreach (HallUnit unit in data.HallUnit)
        {
            DeckUnit deckUnit = new();

            UnitDataSO unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{unit.UnitName}");

            deckUnit.HallUnitID = unit.ID;
            deckUnit.Data = unitData;
            deckUnit.DeckUnitUpgradeStat = unit.UpgradedStat;
            deckUnit.IsMainDeck = unit.IsMainDeck;

            foreach (Stigma stigma in unit.Stigmata)
            {
                deckUnit.AddStigma(stigma);
            }

            foreach (UpgradeData upgrade in unit.Upgrades)
            {
                deckUnit.DeckUnitUpgrade.Add(GameManager.Data.UpgradeController.DataToUpgrade(upgrade));
            }

            HallList.Add(deckUnit);
        }
        return HallList;
    }

    public void CreateData()
    {
        // Resources폴더 안에 있는 데이터를 복사하여 저장
        TextAsset text = GameManager.Resource.Load<TextAsset>("Data/OutGameData");
        data = JsonUtility.FromJson<OutGameData>(text.text);

        ReSetOption();
        SaveData();
    }

    public void SetProgressCoin(int num)
    {
        data.ProgressCoin += num;
        SaveData();
    }

    public int GetProgressCoin()
    {
        return data.ProgressCoin;
    }

    public ProgressItem GetProgressItem(int ID)
    {
        return data.ProgressItems.Find(x => x.ID == ID);
    }

    // 현재 확인하는 아이템이 구매 가능한 아이템인지 확인
    public bool GetBuyable(int ID)
    {
        ProgressItem item = GetProgressItem(ID);

        if (GetProgressItem(item.Prequest).IsLock || !GetProgressItem(ID).IsLock)
        {
            return false;
        }

        return true;
    }

    public bool IsUnlockedItem(int ID)
    {
        ProgressItem item = GetProgressItem(ID);

        return item.IsUnlocked;
    }

    public void BuyProgressItem(int ID)
    {
        ProgressItem item = GetProgressItem(ID);
        item.IsLock = false;
        item.IsUnlocked = true;
        SetProgressCoin(-item.Cost);
    }

    public HallUnit FindHallUnitID(int ID)
    {
        return data.HallUnit.Find(x => x.ID == ID);
    }

    public List<HallUnit> FindHallUnitList()
    {
        return data.HallUnit;
    }

    public void AddHallUnit(DeckUnit unit, bool IsBossClear)
    {
        HallUnit newUnit = new();

        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (data.HallUnit.Find(x => x.ID == i) == null)
            {
                newUnit.ID = i;
                break;
            }
        }

        newUnit.UnitName = unit.Data.ID;
        newUnit.UpgradedStat = unit.DeckUnitUpgradeStat;
        newUnit.UpgradedStat.FallCurrentCount = 4-unit.Data.RawStat.FallMaxCount;
        newUnit.IsMainDeck = false;
        newUnit.Stigmata = unit.GetChangedStigma();
        newUnit.Upgrades = unit.GetUpgradeData();

        Debug.Log(newUnit.UnitName);
        data.HallUnit.Add(newUnit);
        SaveData();

        GameManager.Data.GameData.FallenUnits.Clear();

        if (IsBossClear)
        {
            SceneChanger.SceneChange("EndingCreditScene");
        }
        else
        {
            SceneChanger.SceneChange("MainScene");
        }
    }

    public void DoneTutorial(bool isclear)
    {
        data.TutorialClear = isclear;
        SaveData();
    }

    public void ClearPhanuel(bool isclear)
    {
        data.PhanuelClear = isclear;
        SaveData();
    }

    public void ClearHorus(bool isclear)
    {
        data.HorusClear = isclear;
        SaveData();
    }

    public bool isPhanuelClear()
    {
        return data.PhanuelClear;
    }

    public bool isHorusClear()
    {
        return data.HorusClear;
    }

    public bool isTutorialClear()
    {
        return data.TutorialClear;
    }

    public void setNPCQuest()
    {
        data.npcQuest = GameManager.Data.GameData.NpcQuest;
        data.isVisitUpgrade = GameManager.Data.GameData.IsVisitUpgrade;
        data.isVisitStigma = GameManager.Data.GameData.IsVisitStigma;
        data.isVisitDarkShop = GameManager.Data.GameData.IsVisitDarkShop;
        SaveData();
    }
    public NPCQuest getNPCQuest()
    {
        return data.npcQuest;
    }
    public bool getVisitUpgrade() { return data.isVisitUpgrade; }
    public void setVisitUpgrade(bool _isVisit)
    {
        data.isVisitUpgrade = _isVisit;
        SaveData();
    }
    public bool getVisitStigma() { return data.isVisitStigma; }
    public void setVisitStigma(bool _isVisit)
    {
        data.isVisitStigma = _isVisit;
        SaveData();
    }
    public bool getVisitDarkshop() { return data.isVisitDarkShop; }
    public void setVisitDarkshop(bool _isVisit)
    {
        data.isVisitDarkShop = _isVisit;
        SaveData();
    }
    public bool isGameOverCheck() { return data.isGameOver; }
    public void set_isGameOverCheck(bool _isGameOver) {
        data.isGameOver = _isGameOver;
        SaveData();
    }
    public void resetNPCQuest()
    {
        Debug.Log("npc데이터 리셋되었다.");
        data.isVisitUpgrade = false;
        data.isVisitDarkShop = false;
        data.isVisitStigma = false;
        data.npcQuest.ClearQuest();
        SaveData();
    }
    public void RemoveHallUnit(int ID)
    {
        data.HallUnit.Remove(FindHallUnitID(ID));
    }

    public void DeleteAllData() => File.Delete(path);

    private Resolution GetResolution(int width, int height, int refreshRate)
    {
        Resolution resolution = new Resolution();
        resolution.width = width;
        resolution.height = height;
        resolution.refreshRate = refreshRate;
        return resolution;
    }

    private void SetResolution()
    {
        Resolution resolution = resolutions[data.resolution];
        Screen.SetResolution(resolution.width, resolution.height, !data.isWindowed);
        SaveData();
    }

    public void SetWindow(bool isWindowed)
    {
        data.isWindowed = isWindowed;
        SetResolution();
    }

    public void SetResolution(int resolution)
    {
        data.resolution = resolution;
        SetResolution();
    }
    public List<Resolution> GetAllResolution() => resolutions;
    public int GetLanguage() => data.language;
    public int SetLanguage(int language) => data.language = language;
    public int GetResolution() => data.resolution;
    public bool IsWindowed() => data.isWindowed;
    public float GetMasterSoundPower() => data.masterSoundPower;
    public float GetBGMSoundPower() => data.BGMSoundPower;
    public float GetSESoundPower() => data.SESoundPower;
    public float SetMasterSoundPower(float power) => data.masterSoundPower = power;
    public float SetBGMSoundPower(float power) => data.BGMSoundPower = power;
    public float SetSESoundPower(float power) => data.SESoundPower = power;
    public void ReSetOption()
    {
        data.resolution = 0;
        data.isWindowed = false;
        data.masterSoundPower = data.BGMSoundPower = data.SESoundPower = 1f;
    }
}
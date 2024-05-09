using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

//NPC타락퀘스트 & 진척도 & 전당 유닛 등의 인자를 저장 & 불러오는 기능

[Serializable]
public class OutGameData
{
    public string Version;                   // 데이터 버전
    public int ProgressCoin;                 // 진척도 코인
    public List<ProgressItem> ProgressItems; // 진척도 상점의 상품들
    public List<HallUnit> HallUnit;          // 전당 유닛
    
    public bool TutorialClear = false;
    public bool PhanuelClear = false;
    public bool HorusClear = false;

    public bool IsVisitUpgrade = false;
    public bool IsVisitStigma = false;
    public bool IsVisitDarkShop = false;
    public NPCQuest NpcQuest;                //npc타락퀘스트
    public bool IsGameOver = false;          //바로 전에 게임이 오버되었는지 체크
    public bool[] cutSceneData = new bool[Enum.GetValues(typeof(CutSceneType)).Length];        //현재 진행된 컷씬
    public bool IsOnMainTooltipForHorus = false;
    public bool IsOnMainTooltipForPhanuel = false;

    public int language;
    public int resolution;
    public bool IsWindowed;
    public float MasterSoundPower; // 0 ~ 1
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
    public string PrivateKey;     // 고유 Key
    public int ID;                // 전당 내에서 식별을 위한 ID
    public string UnitName;       // 지금은 유닛 이름으로 받고있지만 ID로 받는 기능이 추가되면 변경해야함
    public Stat UpgradedStat;     //업그레이드된 스텟
    public bool IsMainDeck;       //유닛이 메인덱에 포함되었는지 유무 확인
    public List<StigmaSaveData> Stigmata; // 유닛에게 추가된 낙인
    public List<UpgradeData> Upgrades; //유닛에게 추가된 강화
}


public class OutGameDataContainer : MonoBehaviour
{
    private const string encryptionKey = "EncryptOutGameData!@#$%^&*()_+";

    //데모용
    //private const string OutGameDataFileName = "126634399755.dat";

    //정식용
    private const string OutGameDataFileName = "126634399756.dat";

    private SaveVersionController _versionController;

    // 현재 진행중인 게임에서 관리하는 아웃게임데이터
    private OutGameData _data;
    private string _path;
    private List<Resolution> _resolutions;

    public void Init()
    {
        _versionController = new SaveVersionController();

        // 사용자\AppData\localLow에 있는 SaveData.json의 경로
        _path = Path.Combine(Application.persistentDataPath, OutGameDataFileName);

        _resolutions = new() {
            GetResolution(1920, 1080, 144),
            GetResolution(1280, 720, 144),
            GetResolution(720, 480, 144)
        };

        LoadData();
        SetResolution();
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = string.Empty;
        for (int i = 0; i < data.Length; i++)
            result += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        return result;
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_path, EncryptAndDecrypt(json));
    }

    public void LoadData()
    {
        try
        {
            // AppData에 파일이 있으면 OutGameData파일이 있으면 불러오기
            string json = File.ReadAllText(_path);
            _data = JsonUtility.FromJson<OutGameData>(EncryptAndDecrypt(json));

            if (_versionController.CheckNeedMigration() == true)
            {
                // 데이터 무결성 검사
                _versionController.MigrateData();
            }
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

        foreach (HallUnit unit in _data.HallUnit)
        {
            DeckUnit deckUnit = new DeckUnit();

            deckUnit.PrivateKey = unit.PrivateKey;
            deckUnit.HallUnitID = unit.ID;
            deckUnit.Data = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{unit.UnitName}");
            deckUnit.DeckUnitUpgradeStat = unit.UpgradedStat;
            deckUnit.IsMainDeck = unit.IsMainDeck;

            deckUnit.SetStigmaSaveData(unit.Stigmata);
            deckUnit.SetUpgrade(unit.Upgrades);

            HallList.Add(deckUnit);
        }
        return HallList;
    }

    public void CreateData()
    {
        // Resources폴더 안에 있는 데이터를 복사하여 저장
        TextAsset text = GameManager.Resource.Load<TextAsset>("Data/OutGameData");
        _data = JsonUtility.FromJson<OutGameData>(text.text);
        _data.Version = Application.version;
       
        ReSetOption();
        SaveData();
    }

    public void SetProgressCoin(int num)
    {
        _data.ProgressCoin += num;
        SaveData();
    }

    public int GetProgressCoin() => _data.ProgressCoin;

    public ProgressItem GetProgressItem(int ID) => _data.ProgressItems.Find(x => x.ID == ID);

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

    public HallUnit FindHallUnitID(int ID) => _data.HallUnit.Find(x => x.ID == ID);

    public List<HallUnit> FindHallUnitList() => _data.HallUnit;

    public void AddHallUnit(DeckUnit unit)
    {
        HallUnit newUnit = new();

        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (_data.HallUnit.Find(x => x.ID == i) == null)
            {
                newUnit.ID = i;
                break;
            }
        }

        Debug.Log($"{unit.Data.Name} Unit Get");

        unit.DeckUnitUpgradeStat.FallCurrentCount = 0;

        newUnit.PrivateKey = unit.PrivateKey;
        newUnit.UnitName = unit.Data.ID;
        newUnit.UpgradedStat = unit.DeckUnitUpgradeStat;
        newUnit.IsMainDeck = false;
        newUnit.Stigmata = unit.GetStigmaSaveData();
        newUnit.Upgrades = unit.GetUpgradeData();

        _data.HallUnit.Add(newUnit);
        SaveData();

        GameManager.Data.GameData.FallenUnits.Clear();
        SceneChanger.SceneChange("MainScene");
    }

    public void CoverHallUnit(DeckUnit unit)
    {
        HallUnit coverUnit = null;

        foreach (HallUnit hallUnit in _data.HallUnit)
        {
            if (hallUnit.PrivateKey == unit.PrivateKey)
            {
                coverUnit = hallUnit;
                break;
            }
        }

        if (coverUnit == null)
        {
            Debug.LogError("전당에 등록되지 않은 유닛입니다.");
            return;
        }

        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (_data.HallUnit.Find(x => x.ID == i) == null)
            {
                coverUnit.ID = i;
                break;
            }
        }

        Debug.Log($"{unit.Data.Name} Unit Get");

        unit.DeckUnitUpgradeStat.FallCurrentCount = 0;

        coverUnit.UpgradedStat = unit.DeckUnitUpgradeStat;
        coverUnit.IsMainDeck = true;
        coverUnit.Stigmata = unit.GetStigmaSaveData();
        coverUnit.Upgrades = unit.GetUpgradeData();

        SaveData();

        GameManager.Data.GameData.FallenUnits.Clear();
        SceneChanger.SceneChange("MainScene");
    }

    public void DoneTutorial(bool isclear)
    {
        _data.TutorialClear = isclear;
        SaveData();
    }

    public void ClearPhanuel(bool isclear)
    {
        _data.PhanuelClear = isclear;
        SaveData();
    }

    public void ClearHorus(bool isclear)
    {
        _data.HorusClear = isclear;
        SaveData();
    }

    public bool IsPhanuelClear() => _data.PhanuelClear;

    public bool IsHorusClear() => _data.HorusClear;

    public bool IsTutorialClear() => _data.TutorialClear;

    public void SetNPCQuest()
    {
        _data.NpcQuest = GameManager.Data.GameData.NpcQuest;
        _data.IsVisitUpgrade = GameManager.Data.GameData.IsVisitUpgrade;
        _data.IsVisitStigma = GameManager.Data.GameData.IsVisitStigma;
        _data.IsVisitDarkShop = GameManager.Data.GameData.IsVisitDarkShop;
        SaveData();
    }
    public NPCQuest GetNPCQuest() => _data.NpcQuest;

    public bool GetVisitUpgrade() => _data.IsVisitUpgrade;

    public void SetVisitUpgrade(bool isVisit)
    {
        _data.IsVisitUpgrade = isVisit;
        SaveData();
    }
    public bool GetVisitStigma() => _data.IsVisitStigma;

    public void SetVisitStigma(bool isVisit)
    {
        _data.IsVisitStigma = isVisit;
        SaveData();
    }
    public bool GetVisitDarkshop() => _data.IsVisitDarkShop;

    public void SetVisitDarkshop(bool isVisit)
    {
        _data.IsVisitDarkShop = isVisit;
        SaveData();
    }

    public bool IsGameOverCheck() => _data.IsGameOver;

    public void SetIsGameOverCheck(bool isGameOver) {
        _data.IsGameOver = isGameOver;
        SaveData();
    }

    public void ResetNPCQuest()
    {
        Debug.Log("npc데이터 리셋되었다.");
        _data.IsVisitUpgrade = false;
        _data.IsVisitDarkShop = false;
        _data.IsVisitStigma = false;
        _data.NpcQuest.ClearQuest();
        SaveData();
    }
    public void RemoveHallUnit(int ID)
    {
        _data.HallUnit.Remove(FindHallUnitID(ID));
    }

    public void DeleteAllData() => File.Delete(_path);

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
        Resolution resolution = _resolutions[_data.resolution];
        Screen.SetResolution(resolution.width, resolution.height, !_data.IsWindowed);
        SaveData();
    }

    public void SetWindow(bool isWindowed)
    {
        _data.IsWindowed = isWindowed;
        SetResolution();
    }

    public void SetResolution(int resolution)
    {
        _data.resolution = resolution;
        SetResolution();
    }
    public List<Resolution> GetAllResolution() => _resolutions;
    public int GetLanguage() => _data.language;
    public int SetLanguage(int language) => _data.language = language;
    public int GetResolutionIndex() => _data.resolution;
    public Resolution GetResolution() => _resolutions[_data.resolution];
    public bool IsWindowed() => _data.IsWindowed;
    public float GetMasterSoundPower() => _data.MasterSoundPower;
    public float GetBGMSoundPower() => _data.BGMSoundPower;
    public float GetSESoundPower() => _data.SESoundPower;
    public float SetMasterSoundPower(float power) => _data.MasterSoundPower = power;
    public float SetBGMSoundPower(float power) => _data.BGMSoundPower = power;
    public float SetSESoundPower(float power) => _data.SESoundPower = power;
    public void ReSetOption()
    {
        _data.resolution = 0;
        _data.IsWindowed = false;
        _data.MasterSoundPower = _data.BGMSoundPower = _data.SESoundPower = 0.5f;
    }
    public bool GetCutSceneData(CutSceneType cutSceneType) => _data.cutSceneData[(int)cutSceneType];
    public void SetCutSceneData(CutSceneType cutSceneType, bool isDone) => _data.cutSceneData[(int)cutSceneType] = isDone;
    public bool GetIsOnMainTooltipForHorus() => _data.IsOnMainTooltipForHorus;
    public bool GetIsOnMainTooltipForPhanuel() => _data.IsOnMainTooltipForPhanuel;
    public void SetIsOnMainTooltipForHorus(bool isOn) => _data.IsOnMainTooltipForHorus = isOn;
    public void SetIsOnMainTooltipForPhanuel(bool isOn) => _data.IsOnMainTooltipForPhanuel = isOn;
    public string GetVersion() => _data.Version;
    public void SetVersion(string version) => _data.Version = version;
}
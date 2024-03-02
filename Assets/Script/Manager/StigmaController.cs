using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class StigmaController
{
    public StigmaController()
    {
        LoadStigmaList();
    }

    private List<Stigma> _tier1StigmaList = new();
    private List<Stigma> _tier2StigmaList = new();
    private List<Stigma> _tier3StigmaList = new();
    private List<Stigma> _uniqueStigmaList = new();
    private List<Stigma> _harlotStigmaList = new();

    private Dictionary<StigmaEnum, string[]> _lockStigmaDic = new(); // [Key, Value] = [낙인, 해당 낙인은 등장하지 않는 유닛 이름들]

    private void LoadStigmaList()
    {
        //string path = "Assets/Resources/Prefabs/Stigma";
        //string[] files = Directory.GetFiles(path, "*.prefab");
        GameObject[] stigmaList = Resources.LoadAll("Prefabs/Stigma", typeof(GameObject)).Cast<GameObject>().ToArray();

        //foreach (string file in files) 
        foreach (GameObject go in stigmaList)
        {
            //string fileName = Path.GetFileNameWithoutExtension(file);
            //GameObject go = GameManager.Resource.Load<GameObject>("Prefabs/Stigma/" + fileName);

            Stigma stigma = go.GetComponent<Stigma>();

            CheckUnlockedStigma(stigma);

            if (stigma.IsLock)
            {
                continue;
            }

            if (stigma.Tier == StigmaTier.Tier1)
            {
                _tier1StigmaList.Add(stigma);
            }
            else if (stigma.Tier == StigmaTier.Tier2)
            {
                _tier2StigmaList.Add(stigma);
            }
            else if (stigma.Tier == StigmaTier.Tier3)
            {
                _tier3StigmaList.Add(stigma);
            }
            else if (stigma.Tier == StigmaTier.Unique)
            {
                _uniqueStigmaList.Add(stigma);
            }
            else if (stigma.Tier == StigmaTier.Harlot)
            {
                _harlotStigmaList.Add(stigma);
            }
        }

        _lockStigmaDic.Add(StigmaEnum.Hook, new string[] { "흑기사", "쌍생아", "그을린 자", "묘지기", "검병",
            "노동자", "중갑병", "처형자", "암살자", "습격자", "괴인", "엘리우스", "투발카인" });
        _lockStigmaDic.Add(StigmaEnum.Additional_Punishment, new string[] { "그을린 자", "전령", "괴인", "주시자", "압바임", "라헬&레아" });
    }

    // probability는 { 99, 89 } 처럼 2개 인자를 보유
    public Stigma GetRandomStigma(int[] probability)
    {
        Stigma stigma;
        int randNum = Random.Range(0, 100);

        if (_tier3StigmaList.Count > 0 && randNum >= probability[0])
        {
            stigma = _tier3StigmaList[Random.Range(0, _tier3StigmaList.Count)];
        }
        else if (_tier2StigmaList.Count > 0 && randNum >= probability[1])
        {
            stigma = _tier2StigmaList[Random.Range(0, _tier2StigmaList.Count)];
        }
        else
        {
            stigma = _tier1StigmaList[Random.Range(0, _tier1StigmaList.Count)];
        }

        return stigma;
    }

    public Stigma GetRandomStigmaAsUnit(int[] probability, string unitName)
    {
        Stigma stigma;

        do
        {
            stigma = GameManager.Data.StigmaController.GetRandomStigma(probability);
        } while (GameManager.Data.StigmaController.IsLockStigmaAsUnit(stigma, unitName));

        return stigma;
    }

    private bool IsLockStigmaAsUnit(Stigma stigma, string unitName)
    {
        // 검병 튜토리얼 특수 케이스
        if (unitName.Equals("검병"))
        {
            if (stigma.StigmaEnum == StigmaEnum.Tail_Wind && !GameManager.OutGameData.IsTutorialClear())
            {
                Debug.Log($"검병 튜토리얼: {stigma.StigmaEnum} 차단");
                return true;
            }
        }

        // 그 외 체크
        string[] strings;
        if (_lockStigmaDic.TryGetValue(stigma.StigmaEnum, out strings))
        {
            if (strings.Contains(unitName))
            {
                Debug.Log($"{unitName} : {stigma.StigmaEnum} 차단");
                return true;
            }
        }

        return false;
    }

    public Stigma GetRandomHarlotStigma()
    {
        int size = _harlotStigmaList.Count;
        int randNum = Random.Range(0, size);
        return _harlotStigmaList[randNum];
    }

    public List<Stigma> GetHarlotStigmaList() => _harlotStigmaList;

    public void CheckUnlockedStigma(Stigma stigma)
    {
        UnlockStigma(stigma, 20, StigmaEnum.Sin);
        UnlockStigma(stigma, 16, StigmaEnum.Forbidden_Pact);
        UnlockStigma(stigma, 13, StigmaEnum.Teleport);
        //UnlockStigma(stigma, 10, 갈망);
        UnlockStigma(stigma, 7, StigmaEnum.Killing_Spree);
        //UnlockStigma(stigma, 4, 보복);
        UnlockStigma(stigma, 1, StigmaEnum.Destiny);
    }

    public void UnlockStigma(Stigma stigma, int ID, StigmaEnum unlockedstigma)
    {
        if(GameManager.OutGameData.IsUnlockedItem(ID) && stigma.StigmaEnum == unlockedstigma)
        {
            stigma.UnlockStigma();
        }
    }

    public Stigma SaveDataToStigma(StigmaSaveData data)
    {
        if (data.Tier == StigmaTier.Tier1)
        {
            return _tier1StigmaList.Find(stigma => stigma.StigmaEnum == data.StigmaEnum);
        }
        else if (data.Tier == StigmaTier.Tier2)
        {
            return _tier2StigmaList.Find(stigma => stigma.StigmaEnum == data.StigmaEnum);
        }
        else if (data.Tier == StigmaTier.Tier3)
        {
            return _tier3StigmaList.Find(stigma => stigma.StigmaEnum == data.StigmaEnum);
        }
        else if (data.Tier == StigmaTier.Unique)
        {
            return _uniqueStigmaList.Find(stigma => stigma.StigmaEnum == data.StigmaEnum);
        }
        else if (data.Tier == StigmaTier.Harlot)
        {
            return _harlotStigmaList.Find(stigma => stigma.StigmaEnum == data.StigmaEnum);
        }

        return null;
    }
}
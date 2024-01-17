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
    }

    public Stigma GetRandomStigma(string unitName)
    {
        Stigma stigma = null;
        List<int> probability = new List<int>() { 99, 89 };
        int randNum;

        do
        {
            randNum = Random.Range(0, 100);

            if (randNum >= probability[0])
            {
                if (_tier3StigmaList.Count > 0)
                    stigma = _tier3StigmaList[Random.Range(0, _tier3StigmaList.Count)];
            }
            else if (randNum >= probability[1])
            {
                if (_tier2StigmaList.Count > 0)
                    stigma = _tier2StigmaList[Random.Range(0, _tier2StigmaList.Count)];
            }
            else
            {
                if (_tier1StigmaList.Count > 0)
                    stigma = _tier1StigmaList[Random.Range(0, _tier1StigmaList.Count)];
            }
        } while (stigma == null || IsLockStigmaAsUnit(stigma, unitName));

        return stigma;
    }

    private bool IsLockStigmaAsUnit(Stigma stigma, string unitName)
    {
        switch (unitName) 
        {
            case "검병":
                if (stigma.StigmaEnum == StigmaEnum.Tail_Wind && !GameManager.OutGameData.isTutorialClear())
                {
                    Debug.Log($"{stigma.StigmaEnum} 차단");
                    return true;
                }
                break;
        }

        return false;
    }

    public Stigma GetHarlotStigmas()
    {
        int size = _harlotStigmaList.Count;
        int randNum = Random.Range(0, size);
        return _harlotStigmaList[randNum];
    }

    public void CheckUnlockedStigma(Stigma stigma)
    {
        if (GameManager.OutGameData.IsUnlockedItem(1) && stigma.Name == "운명")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(4) && stigma.Name == "보복")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(7) && stigma.Name == "척살")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(10) && stigma.Name == "갈망")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(13) && stigma.Name == "순간이동")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(16) && stigma.Name == "금지된 계약")
        {
            stigma.UnlockStigma();
        }
        if (GameManager.OutGameData.IsUnlockedItem(20) && stigma.Name == "대죄")
        {
            stigma.UnlockStigma();
        }
    }
}
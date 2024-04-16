using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DeckUnit
{
    public int UnitID { get; set; }
    public UnitDataSO Data; // 유닛 기초 정보

    [SerializeField] public Stat DeckUnitUpgradeStat; // 영구 변화 수치
    [SerializeField] public Stat DeckUnitChangedStat; // 일시적 변화 수치, 한 전투 내에서만 적용

    public List<Upgrade> DeckUnitUpgrade = new();
    public Stat DeckUnitStat
    {
        get 
        {
            Stat result = Data.RawStat + DeckUnitUpgradeStat;

            foreach (Upgrade upgrade in DeckUnitUpgrade)
            {
                result += upgrade.UpgradeStat;
            }

            return result;
        }
    }

    public Stat DeckUnitTotalStat => DeckUnitStat + DeckUnitChangedStat;//일시적 변경된 스탯

    public readonly int UpgradedMaxUpgradeCount = 3;
    public readonly int MaxUpgradeCount = 2;

    private List<Stigma> _stigma = new();

    public readonly int MaxStigmaCount = 3;
    private int _stigmaCount => _stigma.Count;

    [HideInInspector] public int HallUnitID;  //전당 내 유닛 구분을 위한 식별 ID
    public bool IsMainDeck = false;
    public bool CanSpawnInEnemyField => CheckStigma(StigmaEnum.Assasination);
    
    public DeckUnit()
    {
        this.UnitID = -1;
        //this.UnitID = GameManager.UnitIDController.GetID();
    }

    public bool CheckStigma(StigmaEnum findStigma)
    {
        foreach (Stigma stigma in GetStigma())
        {
            if (findStigma == stigma.StigmaEnum)
                return true;
        }

        return false;
    }

    public List<Stigma> GetStigma(bool notGetUniqueStigmata = false)
    {
        List<Stigma> stigmataList = new();

        if (!notGetUniqueStigmata)
        {
            foreach (Stigma stigma in Data.UniqueStigma)
            {
                stigmataList.Add(stigma);
            }
        }

        foreach (Stigma stigma in _stigma)
        {
            stigmataList.Add(stigma);
        }

        return stigmataList;
    }

    public void AddStigma(Stigma stigma)
    {
        if (stigma == null)
        {
            Debug.Log("추가하려는 낙인이 null입니다.");
            return;
        }

        if (_stigma.Contains(stigma) || (Data.UniqueStigma != null && Data.UniqueStigma.Contains(stigma)))
        {
            Debug.Log($"이미 장착된 낙인입니다. : {stigma.Name}");
            return;
        }

        int uniqueStigmaCount = 0;
        if (Data.UniqueStigma != null)
            uniqueStigmaCount = Data.UniqueStigma.Count;

        if(_stigma.Count + uniqueStigmaCount >= MaxStigmaCount)
        {
            Debug.Log("최대 낙인 개수");
            return;
        }

        _stigma.Add(stigma);
    }

    public void AddAllStigma(List<Stigma> stigmaList)
    {
        foreach (Stigma stigma in stigmaList)
        {
            AddStigma(stigma);
        }
    }

    public List<StigmaSaveData> GetStigmaSaveData()
    {
        List<StigmaSaveData> saveDataList = new();
        foreach (Stigma stigma in _stigma)
        {
            StigmaSaveData data = new();
            data.StigmaEnum = stigma.StigmaEnum;
            data.Tier = stigma.Tier;

            saveDataList.Add(data);
        }

        return saveDataList;
    }

    public void SetStigmaSaveData(List<StigmaSaveData> dataList)
    {
        foreach (StigmaSaveData data in dataList)
        {
            Stigma stigma = GameManager.Data.StigmaController.SaveDataToStigma(data);

            AddStigma(stigma);
        }
    }

    public void DeleteStigma(Stigma stigma)
    {
        _stigma.Remove(stigma);
    }

    public void ClearStigma() => _stigma.Clear();


    public int GetUnitSize()
    {
        int size = 0;

        for (int i = 0; i < Data.UnitSize.Length; i++)
        {
            if (Data.UnitSize[i])
                size++;
        }

        return size;
    }

    public List<Vector2> GetUnitSizeRange()
    {
        List<Vector2> RangeList = new();

        int Mrow = 5;
        int Mcolumn = 5;

        for (int i = 0; i < Data.UnitSize.Length; i++)
        {
            if (Data.UnitSize[i])
            {
                int x = (i % Mcolumn) - (Mcolumn >> 1);
                int y = -((i / Mcolumn) - (Mrow >> 1));

                Vector2 vec = new(x, y);

                RangeList.Add(vec);
            }
        }

        return RangeList;
    }

    public List<UpgradeData> GetUpgradeData()
    {
        List<UpgradeData> dataList = new();
        foreach (Upgrade upgrade in DeckUnitUpgrade)
        {
            dataList.Add(upgrade.UpgradeData);
        }

        return dataList;
    }

    public void SetUpgrade(List<UpgradeData> dataList)
    {
        foreach (UpgradeData data in dataList)
        {
            DeckUnitUpgrade.Add(GameManager.Data.UpgradeController.DataToUpgrade(data));
        }
    }

    private int _firstTurnDiscount = 0;
    public bool IsDiscount() => _firstTurnDiscount != 0;

    public int GetHallUnitID()
    {
        return HallUnitID;
    }

    public void FirstTurnDiscount()
    {
        _firstTurnDiscount = (DeckUnitTotalStat.ManaCost + 1) / 2;
        DeckUnitChangedStat.ManaCost -= _firstTurnDiscount;
    }

    public void FirstTurnDiscountUndo()
    {
        if (_firstTurnDiscount != 0)
        {
            DeckUnitChangedStat.ManaCost += _firstTurnDiscount;
            _firstTurnDiscount = 0;
        }
    }

    public int GetStigmaCount()
    {
        return _stigmaCount + Data.UniqueStigma.Count;
    }
}
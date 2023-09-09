using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DeckUnit
{
    public UnitDataSO Data; // 유닛 기초 정보
    
    [SerializeField] public Stat DeckUnitUpgradeStat; // 영구 변화 수치
    [SerializeField] public Stat DeckUnitChangedStat; // 일시적 변화 수치, 한 전투 내에서만 적용

    public Stat DeckUnitStat => Data.RawStat + DeckUnitUpgradeStat;//실제 스탯
    public Stat DeckUnitTotalStat => DeckUnitStat + DeckUnitChangedStat;//일시적 변경된 스탯

    private List<Stigma> _stigma =  new();

    private readonly int _maxStigmaCount = 3;

    public List<Stigma> GetStigma()
    {
        List<Stigma> stigmata = new();

        foreach (Stigma stigma in Data.UniqueStigma)
            stigmata.Add(stigma);

        foreach (Stigma stigma in _stigma)
            stigmata.Add(stigma);

        return stigmata;
    }

    public void AddStigma(Stigma stigma)
    {
        if (_stigma.Contains(stigma))
        {
            Debug.Log($"이미 장착된 낙인입니다. : {stigma.Name}");
            return;
        }

        if(_stigma.Count >= _maxStigmaCount)
        {
            Debug.Log("최대 낙인 개수");
            return;
        }

        _stigma.Add(stigma);
    }

    public void ClearStigma() => _stigma.Clear();

    private int _firstTurnDiscount = 0;
    public bool IsDiscount() => _firstTurnDiscount != 0;

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
}
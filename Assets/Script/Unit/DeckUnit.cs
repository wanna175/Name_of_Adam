using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DeckUnit
{
    public UnitDataSO Data; // 유닛 기초 정보
    
    [SerializeField] public Stat ChangedStat; // 영구 변화 수치
    public Stat Stat => Data.RawStat + ChangedStat; // Memo : 나중에 낙인, 버프 추가한 스탯으로 수정
    
    [SerializeField] public List<Passive> Stigma = new List<Passive>();

    private int _maxStigmaCount = 3;

    public void SetStigma()
    {
        Stigma = Stigma.Distinct().ToList();

        foreach (Passive stigma in Data.UniqueStigma)
            AddStigma(stigma);

        foreach (Passive stigma in Stigma)
            AddStigma(stigma);
    }

    public void AddStigma(Passive passive)
    {
        if (Stigma.Contains(passive))
        {
            Debug.Log($"이미 장착된 낙인입니다. : {passive.GetName(true)}");
            return;
        }

        if(Stigma.Count >= _maxStigmaCount)
        {
            Debug.Log("최대 낙인 개수");
            return;
        }

        Stigma.Add(passive);
    }
}
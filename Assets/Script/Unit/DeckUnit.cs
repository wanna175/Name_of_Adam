using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckUnit
{
    public UnitDataSO Data; // 유닛 기초 정보
    
    [SerializeField] public Stat ChangedStat; // 영구 변화 수치
    public Stat Stat => Data.RawStat + ChangedStat; // Memo : 나중에 낙인, 버프 추가한 스탯으로 수정
    
    [SerializeField] private List<낙인> stigmas = new List<낙인>();
    public List<Passive> Stigmata = new List<Passive>();

    private int _maxStigmaCount = 3;

    public void SetStigma()
    {
        foreach(낙인 stigma in Data.Stigma)
            SetStigmaByEnum(stigma);

        foreach (낙인 stigma in stigmas)
            SetStigmaByEnum(stigma);
    }

    // 낙인 수정
    public void SetStigmaByEnum(낙인 stigma)
    {
        if(Stigmata.Count >= _maxStigmaCount)
        {
            Debug.Log($"이미 낙인이 {_maxStigmaCount}개임");
            return;
        }

        switch (stigma)
        {
            case 낙인.가학:
                Stigmata.Add(new 가학());
                Debug.Log("가학 들어감");
                break;
            case 낙인.강림:
                break;
            case 낙인.고양:
                break;
            case 낙인.대죄:
                break;
            case 낙인.자애:
                break;
            case 낙인.처형:
                break;
            case 낙인.흡수:
                break;
        }
    }
}
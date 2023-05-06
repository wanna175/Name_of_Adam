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

    public 낙인 GetRandomStigma()
    {
        return (낙인)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(낙인)).Length));
    }

    public void AddStigma(낙인 stigma)
    {
        SetStigmaByEnum(stigma);
    }

    public Type RemoveRandomStigma()
    {
        if(Stigmata.Count <= 0)
        {
            Debug.Log("삭제할 낙인이 없습니다.");
            return null;
        }

        int num = UnityEngine.Random.Range(0, Stigmata.Count);
        Type removed = Stigmata[num].GetType(); // 지워질 패시브의 정보
        Stigmata.RemoveAt(num);
        return removed;
    }

    // 낙인 수정
    public void SetStigmaByEnum(낙인 stigma)
    {
        if(Stigmata.Count >= _maxStigmaCount)
        {
            Debug.Log($"이미 낙인이 {_maxStigmaCount}개임");
            return;
        }

        Passive newPassive = null;

        switch (stigma)
        {
            case 낙인.가학:
                newPassive = new 가학();
                break;
            case 낙인.강림:
                newPassive = new 강림();
                break;
            case 낙인.고양:
                newPassive = new 고양(); 
                break;
            case 낙인.대죄:
                newPassive = new 대죄();
                break;
            case 낙인.자애:
                newPassive = new 자애();
                break;
            case 낙인.처형:
                newPassive = new 처형();
                break;
            case 낙인.흡수:
                newPassive = new 흡수();
                break;
            case 낙인.오빠:
                newPassive = new 오빠();
                break;
            case 낙인.동생:
                newPassive = new 동생();
                break;
        }

        foreach(Passive passive in Stigmata)
            if(passive.GetType() == newPassive.GetType())
            {
                Debug.Log("이미 장착된 낙인입니다.");
                return;
            }

        Stigmata.Add(newPassive);
        Debug.Log($"{Data.name}에 {newPassive.GetType()} 낙인이 장착되었습니다.");
    }
}
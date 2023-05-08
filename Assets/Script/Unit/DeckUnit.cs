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
        return (낙인)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(낙인)).Length-2));
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

    public 낙인 PassiveToStigma(Passive p)
    {
        if (p.GetType() == new 가학().GetType())
        {
            return 낙인.가학;
        }
        else if (p.GetType() == new 강림().GetType())
        {
            return 낙인.강림;
        }
        else if (p.GetType() == new 고양().GetType())
        {
            return 낙인.고양;
        }
        else if (p.GetType() == new 대죄().GetType())
        {
            return 낙인.대죄;
        }
        else if (p.GetType() == new 자애().GetType())
        {
            return 낙인.자애;
        }
        else if (p.GetType() == new 처형().GetType())
        {
            return 낙인.처형;
        }
        else if (p.GetType() == new 흡수().GetType())
        {
            return 낙인.흡수;
        }

        return 낙인.가학;
    }

    public Sprite GetStigmaImage(낙인 stigma)
    {
        if (stigma == 낙인.처형)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_execution");
        }
        else if (stigma == 낙인.가학)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_sadism");
        }
        else if (stigma == 낙인.고양)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_encourage");
        }
        else if (stigma == 낙인.대죄)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_sin");
        }
        else if (stigma == 낙인.강림)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_advent");
        }
        else if (stigma == 낙인.자애)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_benevolence");
        }
        else if (stigma == 낙인.흡수)
        {
            return GameManager.Resource.Load<Sprite>($"Arts/Stigma/stigma_absorption");
        }

        return null;
    }

    public string GetStigmaText(낙인 stigma)
    {
        if (stigma == 낙인.처형)
        {
            return "처형 - 공격 후 체력이 10 이하인 적을 즉사시킵니다.";
        }
        else if (stigma == 낙인.가학)
        {
            return "가학 - 공격 시 공격력이 3 증가합니다.";
        }
        else if (stigma == 낙인.고양)
        {
            return "고양 - 소환 시 주변 4칸에 있는 아군의 공격력이 5 증가합니다.";
        }
        else if (stigma == 낙인.대죄)
        {
            return "대죄 - 공격 시 적에게 타락도를 1 부여합니다.";
        }
        else if (stigma == 낙인.강림)
        {
            return "강림 - 소환 시 주변 4칸에 있는 적에게 15 데미지를 줍니다.";
        }
        else if (stigma == 낙인.자애)
        {
            return "자애 - 소환 시 주변 8칸에 있는 아군의 체력을 30 회복시킵니다.";
        }
        else if (stigma == 낙인.흡수)
        {
            return "흡수 - 공격 시 입힌 피해의 30 % 를 회복합니다.";
        }

        return null;
    }
}
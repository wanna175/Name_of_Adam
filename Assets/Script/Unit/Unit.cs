using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData Data;
    
    [SerializeField] private Stat _stat;
    public Stat Stat => Data.RawStat; // Memo : 나중에 낙인, 버프 추가한 스탯으로 수정
    
    [SerializeField] private List<Passive> Stigmas = new List<Passive>();

    //변화값
    private Stat changedStat;

    //최종 변화값
    private UnitData ChangedData;
    
    public UnitData GetUnitData()
    {
        ChangedData = Data;

        // 스탯, 낙인, 마나코스트

        ChangedData.AddStat(changedStat);

        return ChangedData;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckUnit
{
    public UnitDataSO Data;
    
    [SerializeField] public Stat ChangedStat;

    public Stat Stat => Data.RawStat + ChangedStat; // Memo : 나중에 낙인, 버프 추가한 스탯으로 수정
    
    [SerializeField] private List<Passive> Stigmata = new List<Passive>();

    // 낙인 수정
    public void ChangeStigma()
    {

    }
}
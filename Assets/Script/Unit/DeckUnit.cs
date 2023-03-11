using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUnit : MonoBehaviour
{
    public UnitDataSO Data;
    
    [SerializeField] public Stat ChangedStat;

    public Stat Stat => Data.RawStat + ChangedStat; // Memo : 나중에 낙인, 버프 추가한 스탯으로 수정
    
    [SerializeField] private List<Passive> Stigmata = new List<Passive>();

    // 낙인 수정
    public void ChangeStigma()
    {

    }
    /*
    public UnitData SetUnitData()
    {
        UnitData result = new UnitData();

        result.Stat = Data.RawStat + ChangedStat;
        result.Stigma = Stigmata;

        result.Name = Data.Name;
        result.Description = Data.Description;
        result.Faction = Data.Faction;
        result.Rarity = Data.Rarity;
        result.Image = Data.Image;
        result.DarkEssenseDrop = Data.DarkEssenseDrop;
        result.DarkEssenseCost = Data.DarkEssenseCost;
        result.BehaviorType = Data.BehaviorType;
        result.TargetType = Data.TargetType;
        result.UnitSkill = Data.UnitSkill;
        result.MoveRange = Data.MoveRange;
        result.AttackRange = Data.AttackRange;

        return result;
    }
    */
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUnit
{
    public string name { get; }
    public int DarkEssence { get; set; }
    public Sprite image { get; }

    public RewardUnit(string name, int DarkEssence,Sprite image)
    {
        this.name = name;
        this.DarkEssence = DarkEssence;
        this.image = image;
    }
} 
public class RewardController
{
    #region 변수
    Dictionary<int, RewardUnit> dic_units;
    int prev_DarkEssence;
    #endregion
    #region 함수
    public void Init(List<DeckUnit> units, int DarkEssence)
    {
        this.dic_units = new Dictionary<int, RewardUnit>();
        this.prev_DarkEssence = DarkEssence;

        foreach (DeckUnit unit in units)
        {
            dic_units.Add(unit.UnitID, new RewardUnit(unit.Data.Name, unit.DeckUnitStat.FallCurrentCount, 
                GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + unit.Data.Name + "_타락")));
        }
    }
    public void RewardSetting(List<DeckUnit> units, UI_RewardScene rewardScene)//리워드씬에서 값 세팅해주기
    {
        Debug.Log("현재 덱에 있는 유닛 갯수 : " + units.Count);
        rewardScene.Init(units.Count,GameManager.Data.DarkEssense - prev_DarkEssence);
        RewardUnit unit,fallunit;
        int Rewardidx = 0;
        for (int i=units.Count-1;i>=0;--i)
        {
            if(dic_units.TryGetValue(units[i].UnitID,out unit))//기존에 있던 친구들
            {
                unit.DarkEssence -= units[i].DeckUnitStat.FallCurrentCount;
                rewardScene.setContent(Rewardidx, unit);
                dic_units.Remove(units[i].UnitID);
            }
            else//새로 플레이어 덱에 들어온 친구들
            {
                
                fallunit = new RewardUnit(units[i].Data.Name, units[i].DeckUnitStat.FallCurrentCount, 
                    GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + units[i].Data.Name + "_타락"));
                rewardScene.setContent(Rewardidx, fallunit);
            }
            Rewardidx++;
        }
        foreach(var u in dic_units)//죽은 친구들
        {
            unit = u.Value;
            Debug.Log("죽은 친구들 : " + unit.name);
        }
    }
   
    #endregion
}

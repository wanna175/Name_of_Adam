using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUnit
{
    public string name { get; }
    public int DarkEssence { get; }
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
            Debug.Log("배틀전아이디" + unit.UnitID + " 이름 " + unit.Data.Name);
        }


        foreach (DeckUnit unit in units)
        {
            dic_units.Add(unit.UnitID, new RewardUnit(unit.Data.Name, unit.DeckUnitStat.FallCurrentCount, 
                GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + unit.Data.Name + "_타락")));
            Debug.Log("배틀전아이디"+unit.UnitID +" 이름 "+ unit.Data.Name);
        }
    }
    public void RewardSetting(List<DeckUnit> units, UI_BattleOver rewardScene)//리워드씬에서 값 세팅해주기
    {
        RewardUnit unit;
        foreach (DeckUnit uni in units)
        {
            Debug.Log("배틀후아이디" + uni.UnitID + " 이름 " + uni.Data.Name);
        }

        for (int i=units.Count-1;i>=0;--i)
        {
            if(dic_units.TryGetValue(units[i].UnitID,out unit))//기존에 있던 친구들
            {
                int reward = unit.DarkEssence - units[i].DeckUnitStat.FallCurrentCount;
                Sprite sp = unit.image;
                dic_units.Remove(units[i].UnitID);
                Debug.Log("기존 유닛정보 : " +unit.name+" 바뀐신앙값 : "+ reward);
            }
            else//새로 플레이어 덱에 들어온 친구들
            {
                Debug.Log("welcome : " + units[i].Data.Name+" id : "+units[i].UnitID);
            }
        }
        foreach(var u in dic_units)//죽은 친구들
        {
            unit = u.Value;
            Debug.Log("죽은 친구들 : " + unit.name);
        }
    }
   
    #endregion
}

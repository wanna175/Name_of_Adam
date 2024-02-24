using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUnit
{
    public string name { get; }
    public int DarkEssence { get; set; }//diff
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
    #region ����
    Dictionary<int, RewardUnit> dic_units;
    int prev_DarkEssence;
    #endregion
    #region �Լ�
    public void Init(List<DeckUnit> units, int DarkEssence)
    {
        this.dic_units = new Dictionary<int, RewardUnit>();
        this.prev_DarkEssence = DarkEssence;

        
        foreach (DeckUnit unit in units)
        {
            dic_units.Add(unit.UnitID, new RewardUnit(unit.Data.Name, unit.DeckUnitStat.FallCurrentCount, unit.Data.CorruptPortraitImage));
        }
    }
    public void RewardSetting(List<DeckUnit> units, UI_RewardScene rewardScene)//����������� �� �������ֱ�
    {
        rewardScene.Init(GameManager.Data.DarkEssense - prev_DarkEssence);
        RewardUnit unit,fallunit;
        for (int i=0;i<units.Count;++i)
        {
            if(dic_units.TryGetValue(units[i].UnitID,out unit))//������ �ִ� ģ����
            {
                unit.DarkEssence -= units[i].DeckUnitStat.FallCurrentCount;
                rewardScene.setContent(i, unit, 4 - units[i].DeckUnitStat.FallCurrentCount,UnitState.Default);
                dic_units.Remove(units[i].UnitID);
            }
            else//���� �÷��̾� ���� ���� ģ����
            {
                
                fallunit = new RewardUnit(units[i].Data.Name, units[i].Data.RawStat.FallMaxCount-units[i].Data.RawStat.FallCurrentCount
                    -units[i].DeckUnitStat.FallCurrentCount, units[i].Data.CorruptPortraitImage);
                rewardScene.setContent(i, fallunit,4 - units[i].DeckUnitStat.FallCurrentCount,UnitState.NEW);
            }
        }
        int idx = units.Count;
        foreach(var u in dic_units)//���� ģ����
        {
            fallunit = new RewardUnit(u.Value.name, 0, u.Value.image);
            rewardScene.setContent(idx++, fallunit, 0, UnitState.Die);
        }
        rewardScene.setFadeIn(units.Count+dic_units.Count);
        dic_units.Clear();
    }
   
    #endregion
}

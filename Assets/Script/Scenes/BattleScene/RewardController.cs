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
            Debug.Log("��Ʋ�����̵�" + unit.UnitID + " �̸� " + unit.Data.Name);
        }


        foreach (DeckUnit unit in units)
        {
            dic_units.Add(unit.UnitID, new RewardUnit(unit.Data.Name, unit.DeckUnitStat.FallCurrentCount, 
                GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + unit.Data.Name + "_Ÿ��")));
            Debug.Log("��Ʋ�����̵�"+unit.UnitID +" �̸� "+ unit.Data.Name);
        }
    }
    public void RewardSetting(List<DeckUnit> units, UI_BattleOver rewardScene)//����������� �� �������ֱ�
    {
        RewardUnit unit;
        foreach (DeckUnit uni in units)
        {
            Debug.Log("��Ʋ�ľ��̵�" + uni.UnitID + " �̸� " + uni.Data.Name);
        }

        for (int i=units.Count-1;i>=0;--i)
        {
            if(dic_units.TryGetValue(units[i].UnitID,out unit))//������ �ִ� ģ����
            {
                int reward = unit.DarkEssence - units[i].DeckUnitStat.FallCurrentCount;
                Sprite sp = unit.image;
                dic_units.Remove(units[i].UnitID);
                Debug.Log("���� �������� : " +unit.name+" �ٲ�žӰ� : "+ reward);
            }
            else//���� �÷��̾� ���� ���� ģ����
            {
                Debug.Log("welcome : " + units[i].Data.Name+" id : "+units[i].UnitID);
            }
        }
        foreach(var u in dic_units)//���� ģ����
        {
            unit = u.Value;
            Debug.Log("���� ģ���� : " + unit.name);
        }
    }
   
    #endregion
}

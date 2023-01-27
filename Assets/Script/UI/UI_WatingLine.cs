using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WatingLine : MonoBehaviour
{
    [SerializeField] List<UI_WatingUnit> WatingUnitList;
    
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitOrderList;
    #endregion

    public void Start()
    {

    }

    public void SetBattleUnitList(List<BattleUnit> list)
    {
        _BattleUnitOrderList = list;
    }

    public void SetWatingLine()
    {
        for (int i = 0; i < 8; i++)
        {   
            if (i < _BattleUnitOrderList.Count)
            {
                WatingUnitList[i].SetUnit(_BattleUnitOrderList[i].BattleUnitSO.sprite);
            }
            else
            {
                WatingUnitList[i].RemoveUnit();
            }
            
        }
    }
}

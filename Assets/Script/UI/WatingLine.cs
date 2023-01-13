using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatingLine : MonoBehaviour
{
    [SerializeField] List<WatingUnit> WatingUnitList;
    
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitOrderList;
    #endregion

    public void SetBattleUnitList(List<BattleUnit> list)
    {
        _BattleUnitOrderList = list;
    }

    public void SetWatingLine()
    {
        int num;
        if (_BattleUnitOrderList.Count < 5)
            num = _BattleUnitOrderList.Count;
        else
            num = 5;

        for (int i = 0; i < num; i++)
        {
            WatingUnitList[i].SetUnit(_BattleUnitOrderList[i].BattleUnitSO.sprite);
        }
    }
}

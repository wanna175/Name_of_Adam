using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingLine : MonoBehaviour
{
    [SerializeField] GameObject P_WaitingUnit;
    private List<UI_WaitingUnit> _WaitingUnitList = new List<UI_WaitingUnit>();
    
    #region BattleUnitList  
    List<BattleUnit> _BattleUnitOrderList;
    #endregion

    private BattleManager _BattleMNG;

    public void Start()
    {
        
    }

    public void Generate_WaitingUnit(int i)
    {
        _WaitingUnitList[i].enabled = true;
    }

    public void SetBattleOrderList()
    {
        _BattleUnitOrderList = _BattleMNG.GetUnitbyOrder();
    }

    public void SetWaitingLine()
    {
        for (int i = 0; i < _BattleUnitOrderList.Count; i++)
        {
            GameObject obj = Instantiate(P_WaitingUnit, transform);
            _WaitingUnitList.Add(obj.GetComponent<UI_WaitingUnit>());
            _WaitingUnitList[i].SetUnit(_BattleUnitOrderList[i].Data.Image);
        }
    }
}

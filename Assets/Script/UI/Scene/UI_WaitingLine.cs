using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingLine : UI_Scene
{
    [SerializeField] GameObject P_WaitingUnit;
    private List<UI_WaitingUnit> _waitingUnitList = new List<UI_WaitingUnit>();
    private Transform _grid;
    List<BattleUnit> _BattleUnitOrderList;

    public void Start()
    {
        _grid = Util.FindChild(gameObject, "Grid", true).transform;
    }

    private void ClearLine()
    {
        for(int i= _waitingUnitList.Count-1; i>=0; i--)
            RemoveUnit(_waitingUnitList[i]);
    }

    public void RemoveUnit(UI_WaitingUnit unit)
    {
        _waitingUnitList.Remove(unit);
        Destroy(unit.gameObject);
    }

    public void RemoveUnit(BattleUnit removeUnit)
    {

    }

    public void AddUnit(BattleUnit addUnit)
    {
        UI_WaitingUnit newUnit = GameManager.Resource.Instantiate("UI/Sub/WaitingUnit", _grid).GetComponent<UI_WaitingUnit>();
        newUnit.SetUnit(addUnit.Data.Image);
        _waitingUnitList.Add(newUnit);
    }

    public void SetBattleOrderList(List<BattleUnit> sortedList)
    {
        _BattleUnitOrderList = sortedList;
    }

    public void SetWaitingLine()
    {
        ClearLine();

        for (int i = 0; i < _BattleUnitOrderList.Count; i++)
            AddUnit(_BattleUnitOrderList[i]);
    }
}

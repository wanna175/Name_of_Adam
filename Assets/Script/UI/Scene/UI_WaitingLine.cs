using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingLine : UI_Scene
{
    private List<UI_WaitingUnit> _waitingUnitList = new List<UI_WaitingUnit>();
    private Transform _grid;
    private bool _turned = false;
    [SerializeField] private GameObject _button1;
    [SerializeField] private GameObject _button2;

    public void Start()
    {
        _grid = Util.FindChild(gameObject, "Grid", true).transform;
        _button1.SetActive(false);
        _button2.SetActive(false);
    }

    public void ButtonActive()
    {
        if (_waitingUnitList.Count >= 5)
        {
            _button1.SetActive(true);
        }
        if (_button1.activeSelf == true)
        {
            _button2.SetActive(false);
        }
        else if (_button2.activeSelf == true)
        {
            _button1.SetActive(false);
        }
    }

    private void AddUnit(BattleUnit addUnit)
    {
        UI_WaitingUnit newUnit = GameManager.Resource.Instantiate("UI/Sub/WaitingUnit", _grid).GetComponent<UI_WaitingUnit>();
        newUnit.SetUnit(addUnit, _turned);
        _waitingUnitList.Add(newUnit);
    }

    public void RemoveUnit(BattleUnit removeUnit)
    {
        for(int i=0; i<_waitingUnitList.Count; i++)
            if(_waitingUnitList[i].GetUnit() == removeUnit)
                DestroyIcon(_waitingUnitList[i]);
    }

    public void SetWaitingLine(List<BattleUnit> orderList)
    {
        ClearLine();
        ButtonActive();
        for (int i = 0; i < orderList.Count; i++)
            AddUnit(orderList[i]);
    }

    private void ClearLine()
    {
        for (int i = _waitingUnitList.Count - 1; i >= 0; i--)
            DestroyIcon(_waitingUnitList[i]);
    }

    public void DestroyIcon(UI_WaitingUnit unit)
    {
        _waitingUnitList.Remove(unit);
        Destroy(unit.gameObject);
    }

    public void OnClickSeeNextUnits()
    {
        _grid.eulerAngles += new Vector3(0f, 180f, 0f);
        if (_turned == false)
            _turned = true;
        else
            _turned = false;
    }
}

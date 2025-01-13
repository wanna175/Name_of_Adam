using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_WaitingLine : UI_Scene
{
    [SerializeField] private Transform _waitingUnitGrid;

    [SerializeField] private GameObject _upButton;
    [SerializeField] private GameObject _downButton;

    [SerializeField] private UI_WaitingPlayer _waitingPlayer;

    [SerializeField] private GameObject _waitingUnitPrefab;

    private List<UI_WaitingUnit> _waitingUnitList = new();
    //private List<(BattleUnit, int?)> _waitingBattleUnitOrderList = new();

    private int _waitingLinePage = 0;
    const int _waitingUnitMaxCount = 6;

    public void Start()
    {
        _downButton.SetActive(false);
        _upButton.SetActive(false);
    }

    private void ButtonActiveCheck()
    {
        if (_waitingUnitList.Count + (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare) ? 1 : 0) > _waitingUnitMaxCount)
        {
            _upButton.SetActive(_waitingLinePage != 0);
            _downButton.SetActive(_waitingUnitList.Count / _waitingUnitMaxCount != _waitingLinePage);
        }
        else
        {
            _downButton.SetActive(false);
            _upButton.SetActive(false);
            _waitingLinePage = 0;
        }

        CheckWaitingLineActive();
    }

    public void AddUnitOrder((BattleUnit, int?) addUnitOrder)
    {
        int insertIndex = 0;

        foreach (UI_WaitingUnit waitingUnit in _waitingUnitList)
        {
            var existingOrder = waitingUnit.GetUnitOrder();

            int existingSpeed = existingOrder.Item2 ?? existingOrder.Item1.BattleUnitTotalStat.SPD;
            int newSpeed = addUnitOrder.Item2 ?? addUnitOrder.Item1.BattleUnitTotalStat.SPD;

            if (newSpeed > existingSpeed) break;
            if (newSpeed < existingSpeed) { insertIndex++; continue; }

            if (addUnitOrder.Item1.Team < existingOrder.Item1.Team) break;
            if (addUnitOrder.Item1.Team > existingOrder.Item1.Team) { insertIndex++; continue; }

            if (addUnitOrder.Item1.Location.y > existingOrder.Item1.Location.y) break;
            if (addUnitOrder.Item1.Location.y < existingOrder.Item1.Location.y) { insertIndex++; continue; }

            if (addUnitOrder.Item1.Location.x < existingOrder.Item1.Location.x) break;

            insertIndex++;
        }

        UI_WaitingUnit newUnit = GameObject.Instantiate(_waitingUnitPrefab, _waitingUnitGrid).GetComponent<UI_WaitingUnit>();
        newUnit.SetUnitOrder(addUnitOrder);
        newUnit.transform.SetSiblingIndex(insertIndex+1);
        _waitingUnitList.Insert(insertIndex, newUnit);

        ButtonActiveCheck();
    }

    public void RemoveUnitOrder((BattleUnit, int?) removeUnitOrder)
    {
        bool isInLine = true;

        foreach (UI_WaitingUnit waitingUnit in _waitingUnitList)
        {
            if (waitingUnit.GetUnitOrder() == removeUnitOrder)
            {
                _waitingUnitList.Remove(waitingUnit);
                waitingUnit.SetAnimatorBool(isInLine ? "isRemove" : "isRemoveInLine", true);

                break;
            }

            isInLine = false;
        }

        ButtonActiveCheck();
    }

    public void RefreshWaitingUnit()
    {
        foreach (UI_WaitingUnit waitingUnit in _waitingUnitList)
        {
            waitingUnit.SetUnitOrder(waitingUnit.GetUnitOrder());
        }

        ButtonActiveCheck();
    }

    public void ResetWaitingLine(List<(BattleUnit, int?)> orderList)
    {
        ClearWaitingLine();

        foreach (var unit in orderList)
            AddUnitOrder(unit);

        ButtonActiveCheck();
    }

    public void UnitOrderChangeCheck(List<(BattleUnit, int?)> orderList)
    {
        bool isChange = false;

        for (int i = 0; i < _waitingUnitList.Count; i++)
        {
            if (_waitingUnitList[i].GetUnitOrder() != orderList[i])
            {
                _waitingUnitList[i].gameObject.SetActive(false);
                isChange = true;
            }
        }

        if (isChange)
        {
            List<UI_WaitingUnit> waitingUnitNewList = new();

            for (int i = 0; i < orderList.Count; i++)
            {
                UI_WaitingUnit waitingUnit = _waitingUnitList.Find(unit => unit.GetUnitOrder() == orderList[i]);
                waitingUnit.transform.SetSiblingIndex(i+1);
                waitingUnit.gameObject.SetActive(true);
                waitingUnitNewList.Add(waitingUnit);
            }

            RefreshWaitingUnit();

            _waitingUnitList = waitingUnitNewList;

            ButtonActiveCheck();
        }
    }

    private void ClearWaitingLine()
    {
        foreach (UI_WaitingUnit unit in transform.GetComponentsInChildren<UI_WaitingUnit>())
        {
            Destroy(unit.gameObject);
        }
        _waitingUnitList.Clear();
    }

    private void CheckWaitingLineActive()
    {
        for (int i = 0; i < _waitingUnitList.Count; i++)
        {
            if (i + (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare) ? 1 : 0) < _waitingLinePage * _waitingUnitMaxCount)
                _waitingUnitList[i].SetAnimatorBool("isDisable", true);
            else
                _waitingUnitList[i].gameObject.SetActive(true);
        }

        SetWaitingPlayer(_waitingLinePage == 0);
    }

    public void OnClickUpButton()
    {
        _waitingLinePage--;
        ButtonActiveCheck();
    }

    public void OnClickDownButton()
    {
        _waitingLinePage++;
        ButtonActiveCheck();
    }

    public void SetWaitingPlayer(bool on)
    {
        if (on && BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            _waitingPlayer.gameObject.SetActive(true);
        }
        else
        {
            _waitingPlayer.SetAnimatorBool("isRemove", true);
        }
    }
}

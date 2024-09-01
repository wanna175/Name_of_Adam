using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_WaitingLine : UI_Scene
{
    [SerializeField] private Transform _waitingUnitGrid;

    [SerializeField] private GameObject _upButton;
    [SerializeField] private GameObject _downButton;

    [SerializeField] private GameObject _waitingUnitPrefab;

    private List<UI_WaitingUnit> _waitingUnitList = new();
    private List<BattleUnit> _waitingBattleUnitList = new();
    private int _waitingLinePage = 0;

    const int _waitingUnitMaxCount = 6;

    public void Start()
    {
        _downButton.SetActive(false);
        _upButton.SetActive(false);
    }

    private void ButtonActive()
    {
        if (_waitingUnitList.Count > _waitingUnitMaxCount)
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

    private void AddUnit(BattleUnit addUnit)
    {
        UI_WaitingUnit newUnit = GameObject.Instantiate(_waitingUnitPrefab, _waitingUnitGrid).GetComponent<UI_WaitingUnit>();
        newUnit.SetUnit(addUnit);
        _waitingUnitList.Add(newUnit);
    }

    public void SetWaitingLine(List<BattleUnit> orderList)
    {
        if (_waitingBattleUnitList.SequenceEqual(orderList) && 
            _waitingBattleUnitList.Select(unit => unit.Team).SequenceEqual(orderList.Select(unit => unit.Team)))
        {
            return;
        }

        ClearWaitingLine();

        foreach (BattleUnit unit in orderList)
            AddUnit(unit);

        ButtonActive();

        _waitingBattleUnitList = orderList;
    }

    private void ClearWaitingLine()
    {
        foreach (UI_WaitingUnit unit in _waitingUnitList)
        {
            Destroy(unit.gameObject);
        }
        _waitingUnitList.Clear();
    }

    private void CheckWaitingLineActive()
    {
        for (int i = 0; i < _waitingUnitList.Count; i++)
        {
            _waitingUnitList[i].gameObject.SetActive(i >= _waitingLinePage * _waitingUnitMaxCount);
        }
    }

    public void OnClickUpButton()
    {
        _waitingLinePage--;
        ButtonActive();
    }

    public void OnClickDownButton()
    {
        _waitingLinePage++;
        ButtonActive();
    }

    void SlideMove()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float moveDuration = 2f;
        float distanceToMove = 80f;

        Vector3 targetPosition = _waitingUnitGrid.transform.position - new Vector3(distanceToMove, 0, 0);
        Vector3 initialPosition = _waitingUnitGrid.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            _waitingUnitGrid.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _waitingUnitGrid.transform.position = initialPosition;
    }
}

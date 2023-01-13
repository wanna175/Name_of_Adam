using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleEngageManager : MonoBehaviour
{
    private BattleCutSceneManager _CutSceneMNG;
    private BattleDataManager _BattleDataMNG;

    private List<BattleUnit> _BattleUnitOrderList;

    //배틀 시작이 가능한 상태인가?
    private bool _CanBattle = false;
    int _unitListIndex = 0;

    private void Start()
    {
        _CutSceneMNG = GameManager.Instance.BattleMNG.CutSceneMNG;
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;

    }

    public void EngageStart()
    {
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌

        // 턴 시작 전에 다시한번 순서를 정렬한다.
        BattleOrderReplace();
        _BattleDataMNG.FieldMNG.FieldClear();
    }

    public void EngageEnd()
    {
        //UI 들어감
        //UI 사용 불가
        _BattleDataMNG.ManaMNG.AddMana(2);
    }

    // BattleUnitList를 정렬
    // 1. 스피드 높은 순으로, 2. 같을 경우 왼쪽 위부터 오른쪽으로 차례대로
    public void BattleOrderReplace()
    {
        _BattleUnitOrderList = _BattleDataMNG.BattleUnitMNG.BattleUnitList.OrderByDescending(unit => unit.GetSpeed())
            .ThenByDescending(unit => unit.UnitMove.LocY)
            .ThenBy(unit => unit.UnitMove.LocX)
            .ToList();

        //foreach(BattleUnit t in BattleUnitList)
        //{
        //    Debug.Log("Speed : " + t.GetSpeed() + ", Y : " + t.LocY + ", X : " + t.LocX);
        //}
    }


    // 턴 진행
/*
    public void TurnStart()
    {
        // 턴 시작이 가능한 상태라면
        if (_CanBattle)
        {
            _CanBattle = false;
            _unitListIndex = 0;
            
            // 턴 시작 전에 다시한번 순서를 정렬한다.
            _BattleUnitList = _BattleDataMNG.BattleUnitMNG.BattleUnitList;
            _BattleDataMNG.BattleUnitMNG.BattleOrderReplace();
            _BattleDataMNG.FieldMNG.FieldClear();

            //UseUnitSkill();
        }
    }

    // BattleUnitList의 첫 번째 요소부터 순회
    // 다음 차례의 공격 호출은 CutSceneMNG의 ZoomOut에서 한다.
    // 나중에 죽이는 로직 구현 필요
    public void UseUnitSkill()
    {
        // index가 리스트의 범위를 넘지 않는다면 use를 실행
        if (_unitListIndex < _BattleUnitList.Count)
        {
            if (0 < _BattleUnitList[_unitListIndex].UnitAction.CurHP)
            {
                _BattleUnitList[_unitListIndex].use();
                _unitListIndex++;
            }
            else
            {
                _unitListIndex++;
                UseUnitSkill();
            }
        }
        else
        {
            TurnEnd();
        }

    }
*/
    void TurnEnd()
    {
        DestroyDeadUnit();
        _CanBattle = false;
    }


    void DestroyDeadUnit()
    {
        foreach (BattleUnit unit in _BattleDataMNG.BattleUnitMNG.BattleUnitList)
        {
            if (unit.UnitAction.CurHP <= 0)
            {
                unit.UnitAction.UnitDestroy();
            }
        }
    }
}

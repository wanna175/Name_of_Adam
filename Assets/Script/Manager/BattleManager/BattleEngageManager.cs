using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleEngageManager : MonoBehaviour
{
    private BattleCutSceneManager _CutSceneMNG;
    private BattleDataManager _BattleDataMNG;

    private List<BattleUnit> _BattleUnitOrderList;
    
    int _unitListIndex = 0;

    private void Start()
    {
        _CutSceneMNG = GameManager.Instance.BattleMNG.CutSceneMNG;
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;

        _BattleUnitOrderList = new List<BattleUnit>();
    }

    public void EngageStart()
    {
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌

        _BattleUnitOrderList.Clear();

        foreach(BattleUnit unit in _BattleDataMNG.BattleUnitMNG.BattleUnitList)
        {
            _BattleUnitOrderList.Add(unit);
        }

        // 턴 시작 전에 다시한번 순서를 정렬한다.
        BattleOrderReplace();
        _BattleDataMNG.FieldMNG.FieldClear();

        UseUnitSkill();
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
        _BattleUnitOrderList = _BattleUnitOrderList.OrderByDescending(unit => unit.GetSpeed())
            .ThenByDescending(unit => unit.UnitMove.LocY)
            .ThenBy(unit => unit.UnitMove.LocX)
            .ToList();
    }

    // BattleUnitList의 첫 번째 요소부터 순회
    // 다음 차례의 공격 호출은 CutSceneMNG의 ZoomOut에서 한다.
    public void UseUnitSkill()
    {
        DestroyDeadUnit();

        if (_BattleUnitOrderList.Count <= 0)
        {
            EngageEnd();
            return;
        }

        if (0 < _BattleUnitOrderList[0].UnitAction.CurHP)
        {
            _BattleUnitOrderList[0].use();
            _BattleUnitOrderList.RemoveAt(0);
        }
        else
        {
            UseUnitSkill();
        }
    }

    void DestroyDeadUnit()
    {
        List<BattleUnit> units = _BattleDataMNG.BattleUnitMNG.BattleUnitList;

        for(int i = units.Count-1; 0 <= i; i--)
        {
            if(units[i].UnitAction.CurHP <= 0)
            {
                _BattleUnitOrderList.Remove(units[i]);
                units[i].UnitAction.UnitDestroy();
            }
        }
    }
}

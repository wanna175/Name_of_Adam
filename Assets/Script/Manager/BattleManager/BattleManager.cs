using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    #region BattleDataManager
    private BattleDataManager _BattleDataMNG;
    public BattleDataManager BattleDataMNG => _BattleDataMNG;
    #endregion

    private UI_WatingLine _WatingLine;

    private void Awake()
    {
        _BattleDataMNG = new BattleDataManager();
        
        _BattleUnitOrderList = new List<BattleUnit>();
        _WatingLine = GameManager.Instance.UIMNG.WatingLine;

        PrepareStart();


    }

    #region StageControl
    const bool EngageStage = true;
    const bool PrepareStage = false;

    private List<BattleUnit> _BattleUnitOrderList;


    public void PrepareStart()
    {
        Debug.Log("Prepare Start");
        _BattleDataMNG.SetEngageStage(PrepareStage);

        //UI 튀어나옴
        //GameManager.Instance.InputMNG.Hands.comebackHands();
        //UI가 작동할 수 있게 해줌
    }

    public void PrepareEnd()
    {
        Debug.Log("Prepare End");
        _BattleDataMNG.SetEngageStage(PrepareStage);
        
        //UI 들어감
        //GameManager.Instance.InputMNG.Hands.begoneHands();
        //UI 사용 불가
    }

    public void EngageStart()
    {
        Debug.Log("Engage Start");
        _BattleDataMNG.SetEngageStage(EngageStage);
        
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌

        _BattleUnitOrderList.Clear();

        foreach(BattleUnit unit in _BattleDataMNG.BattleUnitList)
        {
            _BattleUnitOrderList.Add(unit);
        }

        // 턴 시작 전에 다시한번 순서를 정렬한다.
        BattleOrderReplace();
        GameManager.Instance.FieldMNG.FieldClear();

        _WatingLine.SetBattleUnitList(_BattleUnitOrderList);
        _WatingLine.SetWatingLine();

        UseUnitSkill();
    }

    public void EngageEnd()
    {
        Debug.Log("Engage End");
        _BattleDataMNG.SetEngageStage(EngageStage);

        //UI 들어감
        //UI 사용 불가
        _BattleDataMNG.ChangeMana(2);
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
        _WatingLine.SetWatingLine();
        
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
        List<BattleUnit> units = _BattleDataMNG.BattleUnitList;

        for(int i = units.Count-1; 0 <= i; i--)
        {
            if(units[i].UnitAction.CurHP <= 0)
            {
                _BattleUnitOrderList.Remove(units[i]);
                units[i].UnitAction.UnitDestroy();
            }
        }
    }
    #endregion
}
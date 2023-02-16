using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    private BattleDataManager _battleData  = new BattleDataManager();
    public BattleDataManager Data => _battleData;
    private List<BattleUnit> _BattleUnitOrderList = new List<BattleUnit>();

    private UIManager _UIMNG;
    private Field _field;
    public Field Field => _field;

    [SerializeField] private bool TestMode = true;
    private ClickType _clickType;

    private void Awake()
    {
        _UIMNG = GameManager.UI;

        SetupField();

        if (TestMode)
            UnitSpawn();

        StartEnter();
    }

    private void UnitSpawn()
    {
        GetComponent<UnitSpawner>().Init();
    }

    private void SetupField()
    {
        GameObject fieldObject = GameObject.Find("Field");
        
        if (fieldObject != null)
            return;

        _field = GameManager.Resource.Instantiate("Field").GetComponent<Field>();
        _field.SetClickEvent(OnClickTile);
    }

    public void OnClickTile(Vector2 coord)
    {
        //Prepare 페이즈
        if (CurrentPhase == Phase.Prepare)
        {
            BattleUnitFactory(coord);
        }
        //Start 페이즈
        else if (CurrentPhase == Phase.Start)
        {
            BattleUnitFactory(coord);
        }
        //Engage 페이즈
        else
        {
            BattleUnit nowUnit = GetNowUnit();
            List<Vector2> RangeList = Field.Get_Abs_Pos(nowUnit, _clickType);

            Field.ClearAllColor();

            // 범위 밖을 클릭했으면 다시 클릭한다.
            if (!RangeList.Contains(coord))
            {
                SetTileColor(Color.yellow);
                return;
            }
            
            if (_clickType == ClickType.Move)
            {
                Vector2 dest = coord - nowUnit.Location;
                MoveLotate(nowUnit, dest);
            }
            else if (_clickType == ClickType.Attack)
            {
                // 제자리를 클릭했다면 공격하지 않는다.
                if (coord != nowUnit.Location)
                    nowUnit.SkillUse(dump());
                // 공격 실행 후 바로 다음유닛 행동 실행
                UseNextUnit();
            }

            ChangeClickType();
        }
    }

    // *****
    // 임시임시
    // 팩토리는 다른곳으로 빼는걸로
    private void BattleUnitFactory(Vector2 coord)
    {
        //범위 외
        if (Field.IsPlayerRange(coord) == false || Field.GetUnit(coord) != null)
            return;

        // ----------------변경 예정------------------------
        Unit clickedUnit = _UIMNG.Hands.ClickedUnit;
        if (clickedUnit == null)
            return;

        _battleData.ChangeMana(-1 * clickedUnit.Data.ManaCost);

        GameObject BattleUnitPrefab = GameManager.Resource.Instantiate("Units/BaseUnit");
        BattleUnit BattleUnit = BattleUnitPrefab.GetComponent<BattleUnit>();

        BattleUnit.Data = clickedUnit.Data;
        UnitSetting(BattleUnit, coord);

        Data.BattleUnitAdd(BattleUnit);
        _UIMNG.Hands.RemoveHand(_UIMNG.Hands.ClickedHand);
        _UIMNG.Hands.ClearHand();
        // ------------------------------------------------
    }
    public void UnitSetting(BattleUnit _unit, Vector2 coord)
    {
        _unit.setLocate(coord);
        _unit.Init(Team.Player, coord);
        Field.EnterTile(_unit, coord);
        _unit.UnitDeadAction = UnitDeadAction;

        Data.BattleUnitAdd(_unit);
    }
    // 23.02.16 임시 수정
    private void UnitDeadAction(BattleUnit _unit)
    {
        Data.BattleUnitRemove(_unit);
        BattleOrderRemove(_unit);
    }

    #region Phase Control

    private Phase _CurrentPhase;
    public Phase CurrentPhase => _CurrentPhase;

    public void PhaseUpdate()
    {
        if (_CurrentPhase == Phase.Prepare)
        {
            PrepareExit();
            EngageEnter();

            PhaseChanger(Phase.Engage);
        }
        else if (_CurrentPhase == Phase.Engage)
        {
            EngageExit();
            PrepareEnter();

            PhaseChanger(Phase.Prepare);
        }
        else if(_CurrentPhase == Phase.Start)
        {
            StartExit();
            EngageEnter();

            PhaseChanger(Phase.Engage);
        }
    }

    public void PhaseChanger(Phase phase)
    {
        _CurrentPhase = phase;
    }

    public void StartEnter()
    {
        //전투시 맨 처음 Prepare 단계
        Debug.Log("Start Enter");
        PhaseChanger(Phase.Start);
    }

    public void StartExit()
    {
        Debug.Log("Start Exit");
    }

    public void PrepareEnter()
    {
        Debug.Log("Prepare Enter");
        PhaseChanger(Phase.Prepare);
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌
    }

    public void PrepareExit()
    {
        Debug.Log("Prepare Exit");
        //UI 들어감
        //UI 사용 불가
    }

    public void EngageEnter()
    {
        Debug.Log("Engage Enter");
        PhaseChanger(Phase.Engage);
        //UI 튀어나옴
        //UI가 작동할 수 있게 해줌

        _BattleUnitOrderList.Clear();

        foreach(BattleUnit unit in _battleData.BattleUnitList)
        {
            _BattleUnitOrderList.Add(unit);
        }

        // 턴 시작 전에 다시한번 순서를 정렬한다.
        BattleOrderReplace();
        GameManager.Battle.Field.ClearAllColor();

        UseUnitSkill();
    }

    public void EngageExit()
    {
        Debug.Log("Engage Exit");
        //UI 들어감
        //UI 사용 불가
        
        BattleOverCheck();
        _battleData.ChangeMana(2);
        _battleData.TurnPlus();
    }
    #endregion

    public void BattleOverCheck()
    {
        int MyUnit = 0;
        int EnemyUnit = 0;
        Debug.Log(Data.BattleUnitList.Count);
        foreach(BattleUnit BUnit in Data.BattleUnitList)
        {
            if (BUnit.Team == Team.Player)//아군이면
                MyUnit++;
            else
                EnemyUnit++;
        }

        MyUnit += Data.PlayerDeck.Count;
        //EnemyUnit 대기 중인 리스트만큼 추가하기

        if (MyUnit == 0)
        {
            GameOver();
        }
        else if(EnemyUnit == 0)
        {
            BattleOver();
        }
    }

    public void BattleOver()
    {
        Debug.Log("YOU WIN");
    }

    public void GameOver()
    {
        Debug.Log("YOU LOSE");
    }

    // BattleUnitList를 정렬
    // 1. 스피드 높은 순으로, 2. 같을 경우 왼쪽 위부터 오른쪽으로 차례대로
    public void BattleOrderReplace()
    {
        _BattleUnitOrderList = _BattleUnitOrderList.OrderByDescending(unit => unit.GetStat().SPD)
            .ThenByDescending(unit => unit.Location.y)
            .ThenBy(unit => unit.Location.x)
            .ToList();
    }


    public List<BattleUnit> GetUnitbyOrder()
    {
        return _BattleUnitOrderList;
    }


    public void BattleOrderRemove(BattleUnit _unit)
    {
        _BattleUnitOrderList.Remove(_unit);
    }

    public void ChangeClickType()
    {
        if (GetNowUnit() == null)
        {
            _clickType = ClickType.Nothing;
            return;
        }

        if (GetNowUnit().Team == Team.Enemy)
            return;

        _clickType++;

        if (_clickType > ClickType.Attack)
            _clickType = ClickType.Nothing;

        SetTileColor(Color.yellow);
    }

    // BattleUnitList의 첫 번째 요소부터 순회
    // 다음 차례의 공격 호출은 CutSceneMNG의 ZoomOut에서 한다.
    public void UseUnitSkill()
    {
        if (_BattleUnitOrderList.Count <= 0)
        {
            PhaseUpdate();
            return;
        }

        if (0 < _BattleUnitOrderList[0].HP.GetCurrentHP())
        {
            if (_BattleUnitOrderList[0].Team == Team.Enemy)
            {
                Unit_AI_Controller ai = _BattleUnitOrderList[0].GetComponent<Unit_AI_Controller>();
                ai.SetCaster(_BattleUnitOrderList[0]);
                ai.AIAction();
                // Unit의 매니저 제거로 인해 임시로 놓음
                UseNextUnit();
            }
            else
            {
                ChangeClickType();
            }
        }
        else
        {
            UseNextUnit();
        }
    }

    public void UseNextUnit()
    {
        Field.ClearAllColor();
        _BattleUnitOrderList.RemoveAt(0);
        UseUnitSkill();
    }
    
    // 이동 경로를 받아와 이동시킨다
    public void MoveLotate(BattleUnit caster, Vector2 coord)
    {
        Vector2 current = caster.Location;
        Vector2 dest = current + coord;

        Field.MoveUnit(current, dest);
    }

    
    public void SetTileColor(Color clr)
    {
        List<Vector2> rangeList = Field.Get_Abs_Pos(GetNowUnit(), _clickType);
        Field.SetTileColor(rangeList, clr);
    }

    // *****
    // 경유만 하는 함수가 필요할까?
    public void SetUnit(BattleUnit unit, Vector2 coord)
    {
        Field.EnterTile(unit, coord);
    }

    public BattleUnit GetNowUnit()
    {
        if (_BattleUnitOrderList.Count != 0)
            return _BattleUnitOrderList[0];
        return null;
    }
    
    // 임시 메서드
    BattleUnit dump()
    {
        BattleUnit caster = GetNowUnit();

        List<Vector2> rangeList = caster.Data.GetAttackRange();
        BattleUnit hitUnits = new BattleUnit();
        
        BattleUnit unit = _field.GetUnit(caster.SelectTile);

        if (unit != null)
        {
            if (unit.Team != caster.Team)
                hitUnits = unit;
        }
        

        return hitUnits;
    }
}
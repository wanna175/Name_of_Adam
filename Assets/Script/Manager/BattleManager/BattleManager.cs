using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    private BattleDataManager _battleData;
    public BattleDataManager Data => _battleData;

    private UIManager _UIMNG;
    private Field _field;
    private Mana _mana;

    public Field Field => _field;

    private ClickType _clickType;

    private UI_Hands _hands;
    private UI_TurnCount _turnCount;

    private Vector2 coord;

    private bool isEngage = true;

    private void Awake()
    {
        _UIMNG = GameManager.UI;
        _battleData = Util.GetOrAddComponent<BattleDataManager>(gameObject);
        _mana = Util.GetOrAddComponent<Mana>(gameObject);
        _hands = _UIMNG.ShowScene<UI_Hands>();
        //_turnCount = GameManager.UI.ShowScene<UI_TurnCount>();

        PhaseChanger(Phase.SetupField);
    }

    private void Update()
    {
        PhaseUpdate();
    }

    private void SetupField()
    {
        GameObject fieldObject = GameObject.Find("Field");

        if (fieldObject == null)
            fieldObject = GameManager.Resource.Instantiate("Field");

        _field = fieldObject.GetComponent<Field>();
    }

    public void OnClickTile(Tile tile)
    {
        coord = Field.FindCoordByTile(tile);

        if(_CurrentPhase == Phase.Engage && _clickType == ClickType.Engage_Nothing)
        {
            _clickType = ClickType.Move;
        }
        else if(_clickType == ClickType.Before_Attack)
        {
            _clickType = ClickType.Attack;
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
        Unit clickedUnit = _hands.ClickedUnit;
        if (clickedUnit == null)
            return;

        _mana.ChangeMana(-1 * clickedUnit.Data.ManaCost);

        GameObject BattleUnitPrefab = GameManager.Resource.Instantiate("Units/BaseUnit");
        BattleUnit BattleUnit = BattleUnitPrefab.GetComponent<BattleUnit>();

        BattleUnit.Data = clickedUnit.Data;
        UnitSetting(BattleUnit, coord);

        Data.BattleUnitAdd(BattleUnit);
        _hands.RemoveHand(_hands.ClickedHand);
        _hands.ClearHand();
        // ------------------------------------------------
    }

    public void UnitSetting(BattleUnit _unit, Vector2 coord)
    {
        _unit.setLocate(coord);
        _unit.Init(_unit.Team, coord);
        Field.EnterTile(_unit, coord);
        _unit.UnitDeadAction = UnitDeadAction;

        Data.BattleUnitAdd(_unit);
    }
    // 23.02.16 임시 수정
    private void UnitDeadAction(BattleUnit _unit)
    {
        Data.BattleUnitRemove(_unit);
        Data.BattleOrderRemove(_unit);
    }

    #region Phase Control

    private Phase _CurrentPhase;
    public Phase CurrentPhase => _CurrentPhase;

    public void PhaseUpdate()
    {
        switch (CurrentPhase)
        {
            case Phase.SetupField:

                SetupField();

                PhaseChanger(Phase.SpawnEnemyUnit);
                break;

            case Phase.SpawnEnemyUnit:

                GetComponent<UnitSpawner>().SpawnInitialUnit();

                PhaseChanger(Phase.Start);
                break;

            case Phase.Start:

                if (_clickType >= ClickType.Engage_Nothing)
                {
                    PhaseChanger(Phase.Engage);
                }
                
                break;

            case Phase.Engage:
                if(isEngage)
                {
                    isEngage = false;

                    Field.ClearAllColor();

                    // 턴 시작 전에 순서를 정렬한다.

                    Data.BattleUnitOrder();
                }

                if(Data.OrderUnitCount > 0)
                {
                    BattleUnit Unit = Data.GetNowUnit();
                    if (0 < Unit.HP.GetCurrentHP())
                    {
                        if (Unit.Team == Team.Enemy)
                        {
                            Unit.AI.AIAction();
                            Field.ClearAllColor();
                            Data.BattleOrderRemove(Unit);
                        }
                        else
                        {
                            Field.ClearAllColor();
                            if (_clickType == ClickType.Engage_Nothing)
                            {
                                Field.SetTileColor(Unit, Color.yellow, ClickType.Move);
                            }
                            else
                            {
                                Field.SetTileColor(Unit,Color.red, ClickType.Attack);
                            }

                            if (Field.Get_Abs_Pos(Unit, _clickType).Contains(coord))
                            {
                                if (_clickType == ClickType.Move)
                                {
                                    Vector2 dest = coord - Unit.Location;
                                    MoveLocate(Unit, dest);
                                    ChangeClickType(ClickType.Before_Attack);
                                }
                                else if (_clickType == ClickType.Attack)
                                {
                                    // 제자리를 클릭했다면 공격하지 않는다.
                                    if (coord != Unit.Location)
                                        
                                        if(Field.GetUnit(coord) == null)
                                        {
                                            // 공격하지 않음
                                        }
                                        else if (Field.GetUnit(coord).Team == Team.Enemy)
                                        {
                                            Unit.SkillUse(Field.GetUnit(coord));
                                        }
                                        else
                                        {
                                            ChangeClickType(ClickType.Before_Attack);
                                            break;
                                        }
                                    // 공격 실행 후 바로 다음유닛 행동 실행
                                    Field.ClearAllColor();
                                    Data.BattleOrderRemove(Unit);
                                    ChangeClickType(ClickType.Engage_Nothing);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    PhaseChanger(Phase.Prepare);
                    isEngage = true;
                    ChangeClickType(ClickType.Prepare_Nothing);// 턴 확인용 임시
                }

                BattleOverCheck();

                break;

            case Phase.Prepare:

                Debug.Log("Prepare Enter");

                _mana.ChangeMana(2);
                _battleData.TurnPlus();
                //_turnCount.ShowTurn();

                if (_clickType >= ClickType.Engage_Nothing)
                {
                    //PrepareExit();
                    Debug.Log("Prepare Exit");

                    PhaseChanger(Phase.Engage);
                }

                break;

            case Phase.BattlaOver:
                Debug.Log("끝끝끝");

                break;
        }
    }

    public void PhaseChanger(Phase phase)
    {
        _CurrentPhase = phase;
    }
    
    #endregion

    public void BattleOverCheck()
    {
        int MyUnit = 0;
        int EnemyUnit = 0;

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
            Debug.Log("YOU LOSE");
            PhaseChanger(Phase.BattlaOver);

        }
        else if(EnemyUnit == 0)
        {
            Debug.Log("YOU WIN");
            PhaseChanger(Phase.BattlaOver);
        }
        
    }

    public void ChangeClickType(ClickType type)
    {
        _clickType = type;
    }

    public void TurnChange()
    {
        if (_clickType < ClickType.Engage_Nothing)
            _clickType = ClickType.Engage_Nothing;
        else
            _clickType = ClickType.Prepare_Nothing;
    }
    
    // 이동 경로를 받아와 이동시킨다
    private void MoveLocate(BattleUnit caster, Vector2 coord)
    {
        Vector2 current = caster.Location;
        Vector2 dest = current + coord;

        Field.MoveUnit(current, dest);
    }
}
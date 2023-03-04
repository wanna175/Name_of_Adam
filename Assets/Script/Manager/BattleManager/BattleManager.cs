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
    private Field _field;
    public Field Field => _field;
    private Mana _mana;
    public Mana Mana => _mana;
    private PhaseController _phase;

    public ClickType _clickType;

    private UI_Hands _hands;

    private Vector2 coord;

    private void Awake()
    {
        _battleData = Util.GetOrAddComponent<BattleDataManager>(gameObject);
        _mana = Util.GetOrAddComponent<Mana>(gameObject);
        _hands = GameManager.UI.ShowScene<UI_Hands>();
        _phase = new PhaseController();
    }

    private void Update()
    {
        _phase.OnUpdate();
    }

    public void SetupField()
    {
        GameObject fieldObject = GameObject.Find("Field");

        if (fieldObject == null)
            fieldObject = GameManager.Resource.Instantiate("Field");

        _field = fieldObject.GetComponent<Field>();
    }

    public void SpawnInitialUnit()
    {
        GetComponent<UnitSpawner>().SpawnInitialUnit();
    }

    public void EngagePhase()
    {
        if (Data.OrderUnitCount <= 0)
        {
            _phase.ChangePhase(_phase.Prepare);
            ChangeClickType(ClickType.Prepare_Nothing);// 턴 확인용 임시
            return;
        }

        BattleUnit Unit = Data.GetNowUnit();
        if (Unit.HP.GetCurrentHP() <= 0)
            return;

        Field.ClearAllColor();
        if (Unit.Team == Team.Enemy)
        {
            Unit.AI.AIAction();
            
            Data.BattleOrderRemove(Unit);
            BattleOverCheck();
        }
        else
        {
            if (_clickType == ClickType.Engage_Nothing)
            {
                Field.SetTileColor(Unit, Field.MoveColor, ClickType.Move);
            }
            else
            {
                Field.SetTileColor(Unit, Field.AttackColor, ClickType.Attack);
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

                        if (Field.GetUnit(coord) == null)
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
                            return;
                        }
                    // 공격 실행 후 바로 다음유닛 행동 실행
                    Field.ClearAllColor();
                    Data.BattleOrderRemove(Unit);
                    ChangeClickType(ClickType.Engage_Nothing);
                    BattleOverCheck();
                }
            }
        }
    }

    public void OnClickTile(Tile tile)
    {
        coord = Field.FindCoordByTile(tile);

        
        if(_phase.Current == _phase.Engage && _clickType == ClickType.Engage_Nothing)
        {
            _clickType = ClickType.Move;
        }
        else if(_clickType == ClickType.Before_Attack)
        {
            _clickType = ClickType.Attack;
        }

    }

    public void UnitSetting(BattleUnit _unit, Vector2 coord)
    {
        _unit.SetTeam(_unit.Team);
        Field.EnterTile(_unit, coord);
        _unit.UnitDeadAction = UnitDeadAction;

        Data.BattleUnitAdd(_unit);
    }

    private void UnitDeadAction(BattleUnit _unit)
    {
        Data.BattleUnitRemove(_unit);
        Data.BattleOrderRemove(_unit);
    }

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
            _phase.ChangePhase(new BattleOverPhase());

        }
        else if(EnemyUnit == 0)
        {
            Debug.Log("YOU WIN");
            _phase.ChangePhase(new BattleOverPhase());
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
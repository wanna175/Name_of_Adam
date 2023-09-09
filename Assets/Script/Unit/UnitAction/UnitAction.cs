using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    protected BattleDataManager _Data;
    protected Field _field;

    protected BattleUnit _unit;
    public void Init(BattleUnit unit)
    {
        _unit = unit;

        _Data = BattleManager.Data;
        _field = BattleManager.Field;
    }

    public virtual void ActionStart(List<BattleUnit> hits)
    {
        BattleManager.Instance.AttackStart(_unit, hits);
    }

    public virtual void Action(BattleUnit receiver)
    {
        _unit.Attack(receiver, _unit.BattleUnitTotalStat.ATK);
    }

    //공격 범위: 움직이지 않고 공격할 수 있는 범위; Attack Range
    //공격가능 타일: 해당 타일 위로 이동할 경우 공격할 수 있는 타일; Attackable Tile

    protected List<Vector2> UnitInAttackRangeList = new();//
    //공격 범위 내 유닛

    protected List<Vector2> AttackableTileList = new();
    //공격 가능 타일 

    protected List<Vector2> AttackableTileInRangeList = new();
    //공격 가능 + 사거리 내 타일

    readonly protected List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    //상하좌우 foreach용

    protected Dictionary<Vector2, int> TileHPDict = new();

    protected bool SetUnitInAttackRangeList()
    {
        //유닛의 공격 범위 내에 있는 유닛을 리스트에 담는다.
        foreach (Vector2 attackRange in _unit.GetAttackRange())
        {
            Vector2 range = _unit.Location + attackRange;

            if (!_field.IsInRange(range))
                continue;

            if (_field.TileDict[range].UnitExist && _field.GetUnit(range).Team == Team.Player)
            {
                UnitInAttackRangeList.Add(range);

                if (TileHPDict.ContainsKey(range))
                {
                    if (TileHPDict[range] >= _field.GetUnit(range).HP.GetCurrentHP()) //여기서 = 이 문제일 수도
                    {
                        TileHPDict.Remove(range);
                        TileHPDict.Add(range, _field.GetUnit(range).HP.GetCurrentHP());
                    }
                }
                else
                {
                    TileHPDict.Add(range, _field.GetUnit(range).HP.GetCurrentHP());
                }
            }
        }

        return UnitInAttackRangeList.Count > 0;
    }

    protected void SetAttackableTile()
    {
        //모든 공격가능 타일을 리스트에 저장한다.
        foreach (BattleUnit unit in _Data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 range in _unit.GetAttackRange())
                {
                    //공격가능 타일은 공격하는 유닛의 공격 범위의 점 대칭이다. 따라서 -.
                    Vector2 attackableRange = unit.Location - range;
                    if (!_field.IsInRange(attackableRange))
                        continue;

                    AttackableTileList.Add(attackableRange);

                    if (TileHPDict.ContainsKey(attackableRange))
                    {
                        if (TileHPDict[attackableRange] >= unit.HP.GetCurrentHP())
                        {
                            TileHPDict.Remove(attackableRange);
                            TileHPDict.Add(attackableRange, unit.HP.GetCurrentHP());
                        }
                    }
                    else
                    {
                        TileHPDict.Add(attackableRange, unit.HP.GetCurrentHP());
                    }
                }
            }
        }
    }

    protected bool AttackableTileSearch()
    {
        //캐스터의 이동 범위 내에 있는 공격가능 타일을 리스트에 담는다.
        List<Vector2> swapList = new();

        foreach (Vector2 moveRange in _unit.GetMoveRange())
        {
            Vector2 range = _unit.Location + moveRange;

            if (AttackableTileList.Contains(_unit.Location))
            {
                AttackableTileInRangeList.Add(_unit.Location);
            }

            if (AttackableTileList.Contains(range))
            {
                if (_field.TileDict[range].UnitExist && _field.GetUnit(range).Team == Team.Enemy)
                    swapList.Add(range);
                else if (!_field.TileDict[range].UnitExist)
                    AttackableTileInRangeList.Add(range);
            }
        }

        if (AttackableTileInRangeList.Count == 0)//스왑해야만 공격 가능한지 확인
        {
            foreach (Vector2 vec in swapList)
                AttackableTileInRangeList.Add(vec);
        }

        return AttackableTileInRangeList.Count > 0;
    }

    protected List<Vector2> MinHPSearch(List<Vector2> vecList)
    {
        //리스트에서 가장 체력이 낮은 적을 찾는다.
        List<Vector2> minHPList = new();

        int minHP = TileHPDict[vecList[0]];
        int currentHP;

        foreach (Vector2 unit in vecList)
        {
            currentHP = TileHPDict[unit];

            if (minHP > currentHP)
            {
                minHP = currentHP;
                minHPList.Clear();
                minHPList.Add(unit);
            }
            else if (minHP == currentHP)
            {
                minHPList.Add(unit);
            }
        }

        return minHPList;
    }

    protected void MoveUnit(Vector2 moveVector)
    {
        _field.MoveUnit(_unit.Location, moveVector);
    }

    protected void Attack(Vector2 vec)
    {
        List<BattleUnit> hitUnits = new();

        foreach (Vector2 splash in _unit.GetSplashRange(vec, _unit.Location))
        {
            if (!_field.IsInRange(splash + vec))
                continue;

            if (_field.TileDict[splash + vec].UnitExist)
            {
                if (_field.GetUnit(splash + vec).Team == Team.Player)
                    hitUnits.Add(_field.GetUnit(splash + vec));
            }
        }

        ActionStart(hitUnits);
    }

    protected Vector2 NearestEnemySearch()
    {
        Vector2 MyPosition = _unit.Location;

        float minDistance = 100f;

        List<Vector2> nearestEnemy = new();

        foreach (Vector2 vec in AttackableTileList)
        {
            float abs = Mathf.Abs(vec.x - MyPosition.x) + Mathf.Abs(vec.y - MyPosition.y);
            if (minDistance > abs)
            {
                minDistance = abs;
                nearestEnemy.Clear();
                nearestEnemy.Add(vec);
            }
            else if (minDistance == abs)
            {
                nearestEnemy.Add(vec);
            }
        }

        return nearestEnemy[Random.Range(0, nearestEnemy.Count)];
    }

    protected Vector2 MoveDirection(Vector2 destination)
    {
        //가야하는 위치 destination을 받아 상하좌우 중 어디로 갈지를 정해 moveVec으로 리턴한다
        Vector2 MyPosition = _unit.Location;
        float currntMin = 100f;

        List<Vector2> moveVectorList = new();

        foreach (Vector2 direction in UDLR)
        {
            Vector2 vec = new(MyPosition.x + direction.x, MyPosition.y + direction.y);
            float sqr = (vec - destination).sqrMagnitude;

            if (!_field.IsInRange(vec))
                continue;

            if (currntMin > sqr && !_field.TileDict[vec].UnitExist)
            {
                currntMin = sqr;
                moveVectorList.Clear();
                moveVectorList.Add(vec);
            }
            else if (currntMin == sqr)
            {
                moveVectorList.Add(vec);
            }
        }

        if (moveVectorList.Count == 0)
            return MyPosition;
        else
            return moveVectorList[Random.Range(0, moveVectorList.Count)];
    }

    protected bool DirectAttackCheck()
    {
        foreach (BattleUnit unit in _Data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
                return false;
        }

        BattleManager.Instance.DirectAttack();
        BattleManager.Instance.EndUnitAction();
        return true;
    }

    protected void ListClear()
    {
        UnitInAttackRangeList.Clear();
        AttackableTileList.Clear();
        AttackableTileInRangeList.Clear();
        TileHPDict.Clear();
    }

    public virtual void AIMove()
    {
        if (DirectAttackCheck())
            return;

        ListClear();
        SetAttackableTile();

        if (AttackableTileSearch())
        {
            List<Vector2> MinHPUnit = MinHPSearch(AttackableTileInRangeList);

            if (!MinHPUnit.Contains(_unit.Location))
            {
                MoveUnit(MoveDirection(MinHPUnit[Random.Range(0, MinHPUnit.Count)]));
            }
            else
            {
                Debug.Log("제자리");
            }
        }
        else
        {
            MoveUnit(MoveDirection(NearestEnemySearch()));
        }

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public virtual void AISkillUse()
    {
        ListClear();

        if (SetUnitInAttackRangeList())
        {
            List<Vector2> MinHPUnit = MinHPSearch(UnitInAttackRangeList);

            Attack(MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
        }
        else
            BattleManager.Instance.EndUnitAction();
    }
}
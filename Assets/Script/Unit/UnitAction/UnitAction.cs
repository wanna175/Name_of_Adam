using System.Collections.Generic;
using UnityEngine;

public class UnitAction : MonoBehaviour
{
    protected BattleDataManager _data;
    protected Field _field;
    public void Init()
    {
        _data = BattleManager.Data;
        _field = BattleManager.Field;
    }

    public virtual void AIMove(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
            return;

        Dictionary<Vector2, int> attackableTile = GetAttackableTile(attackUnit);
        Dictionary<Vector2, int> inRangeList = AttackableTileSearch(attackUnit, attackableTile);

        if (inRangeList.Count > 0)
        {
            List<Vector2> MinHPUnit = MinHPSearch(inRangeList);

            if (!MinHPUnit.Contains(attackUnit.Location))
            {
                MoveUnit(attackUnit, MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
            }
            else
            {
                Debug.Log("제자리");
            }
        }
        else
        {
            MoveUnit(attackUnit, MoveDirection(attackUnit, NearestEnemySearch(attackUnit)));
        }

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public virtual void AISkillUse(BattleUnit attackUnit)
    {
        Dictionary<Vector2, int> attackableTile = GetAttackableTile(attackUnit);
        Dictionary<Vector2, int> unitInattackRange = GetUnitInAttackRangeList(attackUnit, attackableTile);

        if (unitInattackRange.Count > 0)
        {
            List<Vector2> MinHPUnit = MinHPSearch(unitInattackRange);

            Attack(attackUnit, MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
        }
        else
            BattleManager.Instance.EndUnitAction();
    }

    protected bool DirectAttackCheck()
    {
        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
                return false;
        }

        BattleManager.Instance.DirectAttack();
        BattleManager.Instance.EndUnitAction();
        return true;
    }

    protected Dictionary<Vector2, int> GetAttackableTile(BattleUnit attackUnit)
    {
        //모든 공격가능 타일을 리스트에 저장한다.
        Dictionary<Vector2, int> attackableTile = new();

        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 range in attackUnit.GetAttackRange())
                {
                    //공격가능 타일은 공격하는 유닛의 공격 범위의 점 대칭이다. 따라서 -.
                    Vector2 attackableRange = unit.Location - range;
                    if (!_field.IsInRange(attackableRange))
                        continue;

                    if (attackableTile.ContainsKey(attackableRange))
                    {
                        if (attackableTile[attackableRange] >= unit.GetHP())
                        {
                            attackableTile.Remove(attackableRange);
                            attackableTile.Add(attackableRange, unit.GetHP());
                        }
                    }
                    else
                    {
                        attackableTile.Add(attackableRange, unit.GetHP());
                    }
                }
            }
        }
        
        return attackableTile;
    }

    protected Dictionary<Vector2, int> AttackableTileSearch(BattleUnit attackUnit, Dictionary<Vector2, int> attackableTile)
    {
        //유닛의 이동 범위 내에 있는 공격가능 타일을 리스트에 담는다.
        Dictionary<Vector2, int> swapList = new();
        Dictionary<Vector2, int> inRangeList = new();

        foreach (Vector2 moveRange in attackUnit.GetMoveRange())
        {
            Vector2 range = attackUnit.Location + moveRange;

            if (attackableTile.ContainsKey(range))
            {
                if (range == attackUnit.Location)
                    inRangeList.Add(range, attackableTile[range]);
                else if (_field.TileDict[range].UnitExist && _field.GetUnit(range).Team == Team.Enemy)
                    swapList.Add(range, attackableTile[range]);
                else if (!_field.TileDict[range].UnitExist)
                    inRangeList.Add(range, attackableTile[range]);
            }
        }

        if (inRangeList.Count == 0)//스왑해야만 공격 가능한지 확인
        {
            foreach (Vector2 vec in swapList.Keys)
                inRangeList.Add(vec, attackableTile[vec]);
        }

        return inRangeList;
    }

    protected List<Vector2> MinHPSearch(Dictionary<Vector2, int> HPList)
    {
        //리스트에서 가장 체력이 낮은 공격 가능 타일을 찾는다.
        List<Vector2> minHPList = new();

        int minHP = 10000;
        int currentHP;

        foreach (Vector2 unit in HPList.Keys)
        {
            currentHP = HPList[unit];

            if (currentHP < minHP)
            {
                minHP = currentHP;
                minHPList.Clear();
                minHPList.Add(unit);
            }
            else if (currentHP == minHP)
            {
                minHPList.Add(unit);
            }
        }

        return minHPList;
    }

    protected Vector2 MoveDirection(BattleUnit attackUnit, Vector2 destination)
    {
        //가야하는 위치 destination을 받아 상하좌우 중 어디로 갈지를 정해 moveVec으로 리턴한다
        Vector2 MyPosition = attackUnit.Location;
        float minDistance = 100f;

        List<Vector2> moveVectorList = new();

        List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

        foreach (Vector2 direction in UDLR)
        {
            Vector2 tempPosition = MyPosition + direction;
            if (!_field.IsInRange(tempPosition) || _field.TileDict[tempPosition].UnitExist || !attackUnit.GetMoveRange().Contains(direction))
                continue;

            float distance = (tempPosition - destination).sqrMagnitude;

            if (minDistance > distance)
            {
                minDistance = distance;
                moveVectorList.Clear();
                moveVectorList.Add(tempPosition);
            }
            else if (minDistance == distance)
            {
                moveVectorList.Add(tempPosition);
            }
        }

        if (moveVectorList.Count == 0)
            return MyPosition;
        else
            return moveVectorList[Random.Range(0, moveVectorList.Count)];
    }

    protected Vector2 NearestEnemySearch(BattleUnit attackUnit)
    {
        Vector2 MyPosition = attackUnit.Location;

        float minDistance = 100f;

        List<Vector2> fieldUnit = new();

        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                fieldUnit.Add(unit.Location);
            }
        }

        List<Vector2> nearestEnemy = new();

        foreach (Vector2 tile in fieldUnit)
        {
            float distance = (tile - MyPosition).sqrMagnitude;
            if (minDistance > distance)
            {
                minDistance = distance;
                nearestEnemy.Clear();
                nearestEnemy.Add(tile);
            }
            else if (minDistance == distance)
            {
                nearestEnemy.Add(tile);
            }
        }

        return nearestEnemy[Random.Range(0, nearestEnemy.Count)];
    }

    protected Dictionary<Vector2, int> GetUnitInAttackRangeList(BattleUnit attackUnit, Dictionary<Vector2, int> attackableTile)
    {
        //유닛의 공격 범위 내에 있는 유닛.
        Dictionary<Vector2, int> unitInAttackRangeList = new();

        foreach (Vector2 attackRange in attackUnit.GetAttackRange())
        {
            Vector2 range = attackUnit.Location + attackRange;

            if (!_field.IsInRange(range))
                continue;

            if (_field.TileDict[range].UnitExist && _field.GetUnit(range).Team == Team.Player)
            {
                if (unitInAttackRangeList.ContainsKey(range))
                {
                    if (unitInAttackRangeList[range] >= _field.GetUnit(range).GetHP())
                    {
                        unitInAttackRangeList.Remove(range);
                        unitInAttackRangeList.Add(range, _field.GetUnit(range).GetHP());
                    }
                }
                else
                {
                    unitInAttackRangeList.Add(range, _field.GetUnit(range).GetHP());
                }
            }
        }

        return unitInAttackRangeList;
    }

    protected void Attack(BattleUnit attackUnit, Vector2 vec)
    {
        List<BattleUnit> hitUnits = new();

        foreach (Vector2 splash in attackUnit.GetSplashRange(vec, attackUnit.Location))
        {
            if (!_field.IsInRange(splash + vec))
                continue;

            if (_field.TileDict[splash + vec].UnitExist)
            {
                if (_field.GetUnit(splash + vec).Team == Team.Player)
                    hitUnits.Add(_field.GetUnit(splash + vec));
            }
        }

        ActionStart(attackUnit, hitUnits);
    }

    public virtual bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) => false;

    public virtual void ActionStart(BattleUnit attackUnit, List<BattleUnit> hits) => BattleManager.Instance.AttackStart(attackUnit, hits);
    public virtual void Action(BattleUnit attackUnit, BattleUnit receiver) => attackUnit.Attack(receiver, attackUnit.BattleUnitTotalStat.ATK);
    protected void MoveUnit(BattleUnit moveUnit, Vector2 moveVector) => BattleManager.Instance.MoveUnit(moveUnit, moveVector);
}
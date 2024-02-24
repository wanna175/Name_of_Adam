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
        if (!DirectAttackCheck())
        {
            if (attackUnit.Data.UnitMoveType != UnitMoveType.UnitMove_None)
            {
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
                        //���ڸ�
                    }
                }
                else
                {
                    MoveUnit(attackUnit, MoveDirection(attackUnit, NearestEnemySearch(attackUnit)));
                }
            }
        }

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public virtual void AISkillUse(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
        {
            BattleManager.Instance.DirectAttack(attackUnit);
            return;
        }

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

        return true;
    }

    protected Dictionary<Vector2, int> GetAttackableTile(BattleUnit attackUnit)
    {
        //��� ���ݰ��� Ÿ���� ����Ʈ�� �����Ѵ�.
        Dictionary<Vector2, int> attackableTile = new();

        foreach (BattleUnit unit in _data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 range in attackUnit.GetAttackRange())
                {
                    //���ݰ��� Ÿ���� �����ϴ� ������ ���� ������ �� ��Ī�̴�. ���� -.
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
        //������ �̵� ���� ���� �ִ� ���ݰ��� Ÿ���� ����Ʈ�� ��´�.
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

        if (inRangeList.Count == 0)//�����ؾ߸� ���� �������� Ȯ��
        {
            foreach (Vector2 vec in swapList.Keys)
                inRangeList.Add(vec, attackableTile[vec]);
        }

        return inRangeList;
    }

    protected List<Vector2> MinHPSearch(Dictionary<Vector2, int> HPList)
    {
        //����Ʈ���� ���� ü���� ���� ���� ���� Ÿ���� ã�´�.
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
        //�����ϴ� ��ġ destination�� �޾� �����¿� �� ���� ������ ���� moveVec���� �����Ѵ�
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
        //������ ���� ���� ���� �ִ� ����.
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

        ActionStart(attackUnit, hitUnits, vec);
    }

    public virtual bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count == 0)
            return false;

        BattleManager.Instance.AttackStart(attackUnit, hits);
        return true;
    }

    public virtual void SetValue(string sender, int value) { }
    public virtual void SetUnit(string sender, BattleUnit unit) { }
    public virtual bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) => false;
    public virtual void Action(BattleUnit attackUnit, BattleUnit receiver) => attackUnit.Attack(receiver, attackUnit.BattleUnitTotalStat.ATK);
    protected void MoveUnit(BattleUnit moveUnit, Vector2 moveVector) => BattleManager.Instance.MoveUnit(moveUnit, moveVector);
}
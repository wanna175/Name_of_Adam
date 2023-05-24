using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAIController : MonoBehaviour
{
    protected BattleDataManager _Data;
    protected Field _field;

    protected BattleUnit caster;

    //공격 범위: 움직이지 않고 공격할 수 있는 범위; Attack Range
    //공격가능 타일: 해당 타일로 이동할 경우 공격할 수 있는 타일; Attackable Tile

    protected List<Vector2> AttackRangeUnitList = new();//
    //공격 범위 내 유닛

    protected List<Vector2> AttackableTileList = new();
    //공격 가능 타일 

    protected List<Vector2> UnitAttackableTileList = new();
    //공격 가능 + 사거리 내 타일

    protected List<Vector2> FourWay = new() { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
    //상하좌우 foreach용

    protected Dictionary<Vector2, int> TileHPDict = new();

    void Awake()
    {
        _Data = BattleManager.Data;
        _field = BattleManager.Field;
    }

    public void SetCaster(BattleUnit unit)
    {
        caster = unit;
    }

    protected void SetAttackRangeList()
    {
        //캐스터의 공격 범위 내에 있는 유닛을 리스트에 담는다.
        foreach (Vector2 attackRange in caster.GetAttackRange())
        {
            Vector2 range = caster.Location + attackRange;

            if (!_field.IsInRange(range))
            {
                continue;
            }

            if (_field.TileDict[range].UnitExist && _field.GetUnit(range).Team == Team.Player)
            {
                AttackRangeUnitList.Add(range);

                if (TileHPDict.ContainsKey(range))
                {
                    if (TileHPDict[range] >= _field.GetUnit(range).HP.GetCurrentHP())
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
    }

    protected void SetAttackableTile()
    {
        //모든 공격가능 타일을 리스트에 저장한다.
        foreach (BattleUnit unit in _Data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 range in caster.GetAttackRange())
                {
                    //공격가능 타일은 공격하는 유닛의 공격 범위의 점 대칭이다. 따라서 -.
                    Vector2 attackableRange = unit.Location - range;
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

    protected void AttackableTileSearch()
    {
        //캐스터의 공격 범위 내에 있는 유닛을 리스트에 담는다.
        List<Vector2> swapList = new List<Vector2>();

        foreach (Vector2 moveRange in caster.GetMoveRange())
        {
            Vector2 range = caster.Location + moveRange;

            if (!_field.IsInRange(range))
            {
                continue;
            }

            if (AttackableTileList.Contains(range))
            {
                if (_field.TileDict[range].UnitExist)
                {
                    swapList.Add(range);
                }
                else
                {
                    UnitAttackableTileList.Add(range);
                }
            }

            if (UnitAttackableTileList.Count == 0)
            {
                foreach (Vector2 vec in swapList)
                    UnitAttackableTileList.Add(vec);
            }
        }
    }

    protected Vector2 MinHPSearch(List<Vector2> vecList)
    {
        //리스트에서 가장 체력이 낮은 적을 찾는다.
        List<Vector2> minHPList = new List<Vector2>();

        int minHP = TileHPDict[vecList[0]];

        foreach (Vector2 unit in vecList)
        {
            int currentHP = TileHPDict[unit];

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

        return minHPList[Random.Range(0, minHPList.Count)];
    }

    protected void MoveUnit(Vector2 moveVector)
    {
        _field.MoveUnit(caster.Location, moveVector);
    }

    protected void Attack(Vector2 vec)
    {
        List<BattleUnit> hitUnits = new List<BattleUnit>();

        foreach (Vector2 splash in caster.GetSplashRange(vec, caster.Location))
        {
            if (!_field.IsInRange(splash + vec))
                continue;

            if (_field.TileDict[splash + vec].UnitExist)
            {
                if (_field.GetUnit(splash + vec).Team == Team.Player)
                    hitUnits.Add(_field.GetUnit(splash + vec));
            }
        }
        BattleManager.Instance.AttackStart(caster, hitUnits);
    }

    protected Vector2 NearestEnemySearch()
    {
        Vector2 MyPosition = caster.Location;

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
        Vector2 MyPosition = caster.Location;
        float currntMin = 100f;

        List<Vector2> moveVectorList = new();

        foreach (Vector2 direction in FourWay)
        {
            Vector2 Vec = new(MyPosition.x + direction.x, MyPosition.y + direction.y);
            float sqr = (Vec - destination).sqrMagnitude;

            if (!BattleManager.Field.IsInRange(Vec))
                continue;

            if (currntMin > sqr)
            {
                currntMin = sqr;
                moveVectorList.Clear();
                if (!_field.TileDict[Vec].UnitExist)
                {
                    moveVectorList.Add(Vec);
                }
            }
            else if (currntMin == sqr)
            {
                if (!_field.TileDict[Vec].UnitExist)
                {
                    moveVectorList.Add(Vec);
                }
            }
        }

        if (moveVectorList.Count == 0)
        {
            return MyPosition;
        }
        else
        {
            return moveVectorList[Random.Range(0, moveVectorList.Count)];
        }
    }

    protected bool DirectAttackCheck()//임시 삭제
    {
        int playerUnit = 0;

        foreach (BattleUnit unit in _Data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
                playerUnit++;
        }

        if (playerUnit == 0)
        {
            BattleManager.Instance.DirectAttack();
            BattleManager.Instance.EndUnitAction();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void ListClear()
    {
        AttackRangeUnitList.Clear();
        AttackableTileList.Clear();
        UnitAttackableTileList.Clear();
        TileHPDict.Clear();
    }

    public virtual void AIAction()
    {
        if (DirectAttackCheck())
            return;

        SetAttackRangeList();

        if (AttackRangeUnitList.Count > 0)
        {
            Attack(MinHPSearch(AttackRangeUnitList));
        }
        else
        {
            SetAttackableTile();
            AttackableTileSearch();

            if (UnitAttackableTileList.Count > 0)
            {
                MoveUnit(MinHPSearch(UnitAttackableTileList));

                SetAttackRangeList();
                Attack(MinHPSearch(AttackRangeUnitList));
            }
            else
            {
                MoveUnit(MoveDirection(NearestEnemySearch()));
                BattleManager.Instance.EndUnitAction();
            }
        }
        ListClear();
    }

    public virtual void AIMove()
    {
        ListClear();
        SetAttackRangeList();

        if (AttackRangeUnitList.Count > 0)
        {
            MoveUnit(caster.Location);
        }
        else
        {
            SetAttackableTile();
            AttackableTileSearch();

            if (UnitAttackableTileList.Count > 0)
            {
                MoveUnit(MinHPSearch(UnitAttackableTileList));
            }
            else
            {
                MoveUnit(MoveDirection(NearestEnemySearch()));
            }
        }

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public virtual void AISkillUse()
    {
        ListClear();
        SetAttackRangeList();

        if (AttackRangeUnitList.Count > 0)
        {
            Attack(MinHPSearch(AttackRangeUnitList));
        }
        else
            BattleManager.Instance.EndUnitAction();
    }

}

public class CommonUnitAIController : UnitAIController
{
    public override void AIAction()
    {
        base.AIAction();
    }
    public override void AIMove()
    {
        base.AIMove();
    }

    public override void AISkillUse()
    {
        base.AISkillUse();
    }
}
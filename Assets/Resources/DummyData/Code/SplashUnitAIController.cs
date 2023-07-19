using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashUnitAIController : UnitAIController
{
    Dictionary<Vector2, List<Vector2>> TileSplashDic = new();//이동할 타일, 공격시 데미지 받는 유닛이 있는 타일 리스트
    Dictionary<Vector2, Vector2> TileAttackDic = new();//이동할 타일, 공격할 타일


    protected bool SetSplashAttackableList()
    {
        int MaxSplashUnitNum = 0;

        foreach (Vector2 move in caster.GetMoveRange())
        {
            Vector2 moveDest = caster.Location + move; //이동할 위치

            if (!_field.IsInRange(moveDest) || (_field.TileDict[moveDest].UnitExist && _field.GetUnit(moveDest).Team == Team.Player))
                continue;

            foreach (Vector2 attack in caster.GetAttackRange())
            {
                Vector2 attackDest = moveDest + attack;
                if (!_field.IsInRange(attackDest) || moveDest == attackDest)
                    continue;

                List<Vector2> SplashList = new();

                foreach (Vector2 range in caster.GetSplashRange(attackDest, moveDest))
                {
                    Vector2 splash = attackDest + range;

                    if (_field.IsInRange(splash) && _field.TileDict[splash].UnitExist && _field.GetUnit(splash).Team == Team.Player)
                        SplashList.Add(splash);
                }

                if (SplashList.Count > MaxSplashUnitNum)
                {
                    TileSplashDic.Clear();
                    TileSplashDic.Add(moveDest, SplashList);

                    TileAttackDic.Clear();
                    TileAttackDic.Add(moveDest, attackDest);

                    MaxSplashUnitNum = SplashList.Count;
                }
                else if (SplashList.Count == MaxSplashUnitNum && SplashList.Count != 0)
                {
                    if (TileSplashDic.ContainsKey(moveDest))
                    {
                        if (ListMinHP(TileSplashDic[moveDest]) > ListMinHP(SplashList))
                        {
                            TileSplashDic.Clear();
                            TileSplashDic.Add(moveDest, SplashList);

                            TileAttackDic.Clear();
                            TileAttackDic.Add(moveDest, attackDest);
                        }
                    }
                }
            }
        }

        return TileSplashDic.Count > 0;
    }

    protected Vector2 SplashAttackableTileSearch()
    {
        List<Vector2> destVec = new();
        int minHP = 999999;
        int currentHP;

        foreach (Vector2 move in caster.GetMoveRange())
        {
            Vector2 loc = caster.Location + move;
            Debug.Log("loc" + loc + " caster.Location " + caster.Location + "move" + move);
            
            if (!TileSplashDic.ContainsKey(loc)) 
                continue;

            if (loc == caster.Location)//제자리 우선
            {
                Debug.Log("제자리 우선");
                return loc;
            }
                


            currentHP = ListMinHP(TileSplashDic[loc]);

            if (currentHP < minHP)
            {
                destVec.Clear();
                destVec.Add(loc);
                minHP = currentHP;
            }
            else if (currentHP == minHP)
            {
                destVec.Add(loc);
            }
        }

        return destVec[Random.Range(0, destVec.Count)];
    }

    protected int ListMinHP(List<Vector2> list)
    {
        int minHP = _field.GetUnit(list[0]).HP.GetCurrentHP();
        foreach (Vector2 vec in list)
        {
            if (minHP > _field.GetUnit(vec).HP.GetCurrentHP())
                minHP = _field.GetUnit(vec).HP.GetCurrentHP();
        }

        return minHP;
    }

    protected void SplashListClear()
    {
        ListClear();
        TileSplashDic.Clear();
        TileAttackDic.Clear();
    }

    public override void AIAction()
    {
        if (SetSplashAttackableList())
        {
            MoveUnit(SplashAttackableTileSearch());
        }
        else
        {
            SetAttackableTile();
            MoveUnit(MoveDirection(NearestEnemySearch()));
        }
        SplashListClear();
    }

    public override void AIMove()
    {
        if (DirectAttackCheck())
            return;

        SplashListClear();

        if (SetSplashAttackableList())
        {
            MoveUnit(SplashAttackableTileSearch());
        }
        else
        {
            SetAttackableTile();

            if (AttackableTileSearch())
            {
                //MoveUnit(MinHPSearch(AttackableTileInRangeList));
            }
            else
            {
                MoveUnit(MoveDirection(NearestEnemySearch()));
            }
        }

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public override void AISkillUse()
    {
        if (TileSplashDic.Count > 0)
        {
            Attack(TileAttackDic[SplashAttackableTileSearch()]);
        }
        else
        {
            BattleManager.Instance.EndUnitAction();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashUnitAIController : UnitAIController
{
    Dictionary<Vector2, List<Vector2>> TileSplashDic = new();

    protected void SetSplashAttackableList()
    {
        int splashUnit = -1;

        foreach (Vector2 move in caster.GetMoveRange())
        {
            Vector2 loc = caster.Location + move; //이동할 위치

            if (!_field.IsInRange(loc) || _field.TileDict[loc].UnitExist) continue;

            foreach (Vector2 attack in caster.GetAttackRange())
            {
                Vector2 attackLoc = loc + attack;

                if (!_field.IsInRange(attackLoc) || loc == attackLoc) continue;

                List<Vector2> SplashList = new();
                SplashList.Add(attackLoc);//리스트의 첫번째 값은 공격할 위치

                foreach (Vector2 range in caster.GetSplashRange(attackLoc, loc))
                {
                    Vector2 splash = attackLoc + range;

                    if (!_field.IsInRange(splash)) continue;

                    if (_field.TileDict[splash].UnitExist && _field.GetUnit(splash).Team == Team.Player)
                    {
                        SplashList.Add(splash);
                    }
                }

                if (SplashList.Count <= 1) continue;

                if (SplashList.Count > splashUnit)
                {
                    TileSplashDic.Clear();
                    TileSplashDic.Add(loc, SplashList);
                    splashUnit = SplashList.Count;
                }
                else if (SplashList.Count == splashUnit)
                {//이 부분 좀 이상함 3/25
                    if (TileSplashDic.ContainsKey(loc)) {
                        if (Random.Range(0, 2) == 1)
                        {
                            TileSplashDic.Remove(loc);
                            TileSplashDic.Add(loc, SplashList);
                        }
                    }
                    else TileSplashDic.Add(loc, SplashList);

                }
            }
        }
    }

    protected Vector2 SplashAttackableUnitSearch()
    {
        List<Vector2> destVec = new();
        int minHP = 999999;

        foreach (Vector2 move in caster.GetMoveRange())
        {
            Vector2 loc = caster.Location + move;
            
            if (!TileSplashDic.ContainsKey(loc)) continue;

            if (loc == caster.Location)  return loc;

            for (int i = 1; i < TileSplashDic[loc].Count; i++)
            {
                Vector2 splash = TileSplashDic[loc][i];

                if (_field.GetUnit(splash).HP.GetCurrentHP() < minHP)
                {
                    destVec.Clear();
                    destVec.Add(loc);
                    minHP = _field.GetUnit(splash).HP.GetCurrentHP();
                }
                else if (_field.GetUnit(splash).HP.GetCurrentHP() == minHP)
                {
                    destVec.Add(loc);
                }

            }
        }

        return destVec[Random.Range(0, destVec.Count)];
    }

    protected void SplashAttack(Vector2 vec)
    {
        List<BattleUnit> hitUnits = new List<BattleUnit>();

        foreach (Vector2 splash in caster.GetSplashRange(TileSplashDic[vec][0], caster.Location))
        {
            Debug.Log(splash + TileSplashDic[vec][0]);
            //Attack(splash + TileSplashDic[vec][0]);
            hitUnits.Add(_field.GetUnit(splash + TileSplashDic[vec][0]));
        }
        Debug.Log('a');

        BattleManager.Instance.AttackStart(caster, hitUnits);
    }

    protected void SplashListClear()
    {
        ListClear();
        TileSplashDic.Clear();
    }

    public override void AIAction()
    {
        SetSplashAttackableList();
        if (TileSplashDic.Count > 0)
        {
            Vector2 vec = SplashAttackableUnitSearch();
            MoveUnit(vec);
            SplashAttack(vec);
        }
        else
        {
            SetAttackableTile();
            MoveUnit(MoveDirection(NearestEnemySearch()));
        }
        SplashListClear();
    }
}
using System.Collections.Generic;
using UnityEngine;

public class UnitAction_Horus : UnitAction
{
    readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private int _summonOrder = 1;
    private List<BattleUnit> _summonedUnit = new();
    private bool _isSummon = false;

    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
        {
            BattleManager.Instance.EndUnitAction();
            return;
        }

        attackUnit.AnimatorSetBool("isAttack", true);
        BattleManager.Instance.PlayAfterCoroutine(() =>
        {
            attackUnit.AnimatorSetBool("isAttack", false);
        }, 3f);

        BattleManager.Instance.PlayAfterCoroutine(() =>
        {
            SpawnUnitNearEnemy(attackUnit);
            SpawnUnitNearEnemy(attackUnit);
            BattleManager.Instance.EndUnitAction();
        }, 1.5f);
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 0 || _isSummon)
            return false;

        _isSummon = true;

        attackUnit.AnimatorSetBool("isAttack", true);
        BattleManager.Instance.PlayAfterCoroutine(() =>
        {
            attackUnit.AnimatorSetBool("isAttack", false);
        }, 3f);

        BattleManager.Instance.PlayAfterCoroutine(() =>
        {
            SpawnUnit(coord, attackUnit);
            SpawnUnitNearEnemy(attackUnit);
            BattleManager.Instance.EndUnitAction();
        }, 1.5f);

        
        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            BattleManager.Field.TileDict[caster.Location].ExitTile();
            if (caster.Team == Team.Player)
            {
                caster.transform.position = new(-9, 3, 0);
            }
            else
            {
                caster.transform.position = new(9, 3, 0);
            }

            GameManager.Resource.Instantiate("BattleUnits/Horus_UI", caster.transform).GetComponent<Horus_UI>().Init(caster);
        }
        else if ((activeTiming & ActiveTiming.STIGMA) == ActiveTiming.STIGMA)
        {
            if (caster.Team == Team.Player)
            {
                caster.transform.position = new(-9, 3, 0);
                caster.SetFlipX(false);
            }
            else
            {
                caster.transform.position = new(9, 3, 0);
                caster.SetFlipX(true);
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            if (!_isSummon)
            {
                SpawnUnitNearEnemy(caster);
                SpawnUnitNearEnemy(caster);
            }

            _isSummon = false;
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_DEAD) == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                BattleManager.Instance.PlayAfterCoroutine(() =>
                {
                    caster.AnimatorSetBool("isHit", true);
                    caster.GetAttack(-500, null);
                }, 1.5f);

                BattleManager.Instance.PlayAfterCoroutine(() =>
                {
                    caster.AnimatorSetBool("isHit", false);
                }, 1.8f);
            }
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_FALLED) == ActiveTiming.FIELD_UNIT_FALLED)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                caster.ChangeFall(1);
            }
        }

        return false;
    }

    private void SpawnUnitNearEnemy(BattleUnit unit)
    {
        List<Vector2> summonVectorList = new();

        foreach (BattleUnit listUnit in BattleManager.Data.BattleUnitList)
        {
            if (listUnit.Team != unit.Team)
            {
                foreach (Vector2 udrl in UDLR)
                {
                    Vector2 checkVec = listUnit.Location + udrl;

                    if (BattleManager.Field.GetUnit(checkVec) == null && BattleManager.Field.IsInRange(checkVec))
                    {
                        summonVectorList.Add(checkVec);
                    }
                }
            }
        }

        if (summonVectorList.Count > 0)
        {
            SpawnUnit(summonVectorList[Random.Range(0, summonVectorList.Count)], unit);
        }
        else
        {
            for (int i = 0; i < 100; i++) //Àû´çÈ÷ Å« ¹«ÀÛÀ§ ½Ãµµ È½¼öÀÎ 100
            {
                Vector2 randVec = new(Random.Range(0, 6), Random.Range(0, 3));

                if (BattleManager.Field.GetUnit(randVec) == null)
                {
                    SpawnUnit(randVec, unit);
                    break;
                }
            }
        }
    }

    public override void SetUnit(string sender, BattleUnit unit)
    {
        if (sender == "Horus_Egg")
        {
            if (unit.Data.ID == "Èñ»ýÀÇ_²É")
            {
                _summonedUnit.Remove(unit);
            }
            else 
            {
                _summonedUnit.Add(unit);
            }
        }
    }

    private void SpawnUnit(Vector2 spawnLocation, BattleUnit unit)
    {
        _isSummon = true;

        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/Èñ»ýÀÇ_²É");
        sd.location = spawnLocation;
        sd.team = unit.Team;

        BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
        _summonedUnit.Add(spawnUnit);
        spawnUnit.Action.SetUnit("Horus", unit);
        spawnUnit.Action.SetValue("Horus", _summonOrder);

        _summonOrder = ((_summonOrder % 3) + 1);
    }
}
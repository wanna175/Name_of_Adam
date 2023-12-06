using System.Collections.Generic;
using UnityEngine;

public class UnitAction_Horus : UnitAction
{
    readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private List<string> _summonList = new() { "검병", "궁병", "수녀" };
    private int _summonOrder = 0;
    private List<BattleUnit> _summonedUnit = new();
    private bool _isSummon = false;
    private int _deadUnit = 0, _falledUnit = 0;

    public override void AISkillUse(BattleUnit attackUnit)
    {
        SpawnUnitNearEnemy(attackUnit.Team);
        BattleManager.Instance.EndUnitAction();
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 0)
            return false;

        SpawnUnit(coord, attackUnit.Team);
        BattleManager.Instance.EndUnitAction();

        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if (activeTiming == ActiveTiming.SUMMON)
        {
            BattleManager.Field.TileDict[caster.Location].ExitTile();
            if (caster.Team == Team.Player)
            {
                caster.transform.position = new Vector3(-9, 0, 0);
            }
            else
            {
                caster.transform.position = new Vector3(9, 0, 0);

            }
        }
        else if (activeTiming == ActiveTiming.FALLED)
        {
            if (caster.Team == Team.Player)
            {
                caster.transform.position = new Vector3(-9, 0, 0);
            }
            else
            {
                caster.transform.position = new Vector3(9, 0, 0);

            }
        }
        else if (activeTiming == ActiveTiming.ATTACK_TURN_END)
        {
            if (!_isSummon)
            {
                SpawnUnitNearEnemy(caster.Team);
            }

            _isSummon = false;
        }
        else if (activeTiming == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                _deadUnit++;
            }
        }
        else if (activeTiming == ActiveTiming.FIELD_UNIT_FALLED)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                _falledUnit++;
            }
        }
        else if (activeTiming == ActiveTiming.FIELD_ATTACK_TURN_END)
        {
            if (_falledUnit > 0)
            {
                caster.ChangeFall(_falledUnit);
                _falledUnit = 0;
            }

            if (_deadUnit > 0)
            {
                caster.GetAttack(-10 * _deadUnit, null);
                _deadUnit = 0;
            }
        }

        return false;
    }

    private void SpawnUnitNearEnemy(Team attackUnitTeam)
    {
        List<Vector2> summonVectorList = new();

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Team != attackUnitTeam)
            {
                foreach (Vector2 udrl in UDLR)
                {
                    Vector2 checkVec = unit.Location + udrl;

                    if (BattleManager.Field.GetUnit(checkVec) == null && BattleManager.Field.IsInRange(checkVec))
                    {
                        summonVectorList.Add(checkVec);
                    }
                }
            }
        }

        if (summonVectorList.Count > 0)
        {
            SpawnUnit(summonVectorList[Random.Range(0, summonVectorList.Count)], attackUnitTeam);
        }
        else
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 randVec = new(Random.Range(0, 6), Random.Range(0, 3));

                if (BattleManager.Field.GetUnit(randVec) == null)
                {
                    SpawnUnit(randVec, attackUnitTeam);
                    break;
                }
            }
        }
    }
    private void SpawnUnit(Vector2 spawnLocation, Team team)
    {
        _isSummon = true;

        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/오벨리스크");
        sd.location = spawnLocation;
        sd.team = team;

        BattleUnit unit = BattleManager.Spawner.SpawnDataSpawn(sd);
        /*
        UnitAction_Horus_Egg eggAction = unit.Action as UnitAction_Horus_Egg;

        eggAction.SetUnitSO(_summonList[_summonOrder]);
        _summonOrder = (_summonOrder + 1) % 3;

        _summonedUnit.Add(unit);
        */
    }
}
using System.Collections.Generic;
using UnityEngine;

public class UnitAction_Horus : UnitAction
{
    readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private List<string> _summonOrder = new() { "검병", "궁병", "수녀" };
    private List<BattleUnit> _summonedUnit = new();
    private bool _isSummon = false;

    public override void AIMove(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
            return;

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }

    public override void AISkillUse(BattleUnit attackUnit)
    {
        SpawnUnitNearEnemy(attackUnit.Team);
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 0)
            return false;

        SpawnUnit(coord, attackUnit.Team);
        BattleManager.Instance.EndUnitAction();
        _isSummon = true;
        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if (activeTiming == ActiveTiming.SUMMON)
        {
            BattleManager.Field.TileDict[caster.Location].ExitTile();
            caster.transform.position = new Vector3(-10, 0, 00);
        }
        else if (activeTiming == ActiveTiming.ATTACK_TURN_END)
        {
            if (!_isSummon)
            {
                SpawnUnitNearEnemy(caster.Team);
                _isSummon = false;
            }
        }
        else if (activeTiming == ActiveTiming.FIELD_UNIT_DEAD)
        { 
        
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

                    if (BattleManager.Field.GetUnit(checkVec) == null)
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
        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/오벨리스크");
        sd.location = spawnLocation;
        sd.team = team;

        _summonedUnit.Add(BattleManager.Spawner.SpawnDataSpawn(sd));
    }
}
using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Nimrod : UnitAction
{
    //0 index is vary last face, 1 index is second last face
    private int[] _recentFace = {-1, -1};
    //0 = smile, 1 = weep, 2 = mad
    private int _nimrodFace = 0;
    private List<Vector2> _attackTile = new();

    public override void AIMove(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
            return;

        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }
     
    public override void AISkillUse(BattleUnit attackUnit)
    {
        List<BattleUnit> targetUnits = new();
        List<Vector2> emptyTiles = new();

        foreach (Vector2 tile in  _attackTile)
        {
            BattleUnit unitOnTile = BattleManager.Field.GetUnit(tile);

            if (unitOnTile != null && unitOnTile.Team != attackUnit.Team)
            {
                targetUnits.Add(unitOnTile);
            }
            else if (unitOnTile == null)
            {
                emptyTiles.Add(tile);
            }
        }

        if (emptyTiles.Count > 0)
        {
            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/오벨리스크");
            sd.location = emptyTiles[Random.Range(0, emptyTiles.Count)];
            sd.team = attackUnit.Team;

            BattleManager.Spawner.SpawnDataSpawn(sd);
        }
        if (targetUnits.Count > 0)
        {
            BattleManager.Instance.AttackStart(attackUnit, targetUnits);
        }
        else
        {
            BattleManager.Instance.EndUnitAction();
        }

        if (attackUnit.Team == Team.Enemy)
            BattleManager.Field.ClearEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Enemy);
        else if (attackUnit.Team == Team.Player)
            BattleManager.Field.ClearEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Friendly);
    }

    private void NimrodFaceCheck(BattleUnit caster)
    {
        if (_recentFace[0] == -1)
        {
            _nimrodFace = 0;

            _recentFace[0] = _nimrodFace;

            return;
        }

        int count = 0;

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
            {
                count++;
            }
        }

        if (count > 6)
        {
            _nimrodFace = 2;
        }
        else
        {
            if (_recentFace[0] == _recentFace[1])
            {
                _nimrodFace = _recentFace[0] == 0 ? 1 : 0;
            }
            else
            { 
                _nimrodFace = Random.Range(0, 2);
            }
        }

        _recentFace[0] = _nimrodFace;
        _recentFace[1] = _recentFace[0];
    }

    private void SetAttackTile(BattleUnit caster)
    {
        _attackTile.Clear();

        if (_nimrodFace == 0)
        {
            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if (unit.Team != caster.Team)
                {
                    TileAdd(unit.Location);
                }
            }
        }
        else if (_nimrodFace == 1)
        {
            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
                {
                    List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

                    foreach (Vector2 udrl in UDLR)
                    {
                        if (!BattleManager.Field.IsInRange(unit.Location + udrl))
                            continue;

                        BattleUnit udrlUnit = BattleManager.Field.GetUnit(unit.Location + udrl);
                        if (udrlUnit != null && (udrlUnit.Data.ID == "오벨리스크ID" && udrlUnit.Team == caster.Team))
                        {
                            continue;
                        }

                        TileAdd(unit.Location + udrl);
                    }
                }
            }
        }
        else if (_nimrodFace == 2)
        {
            List<Vector2> nonAttackTiles = new();

            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
                {
                    nonAttackTiles.Add(unit.Location);
                }
            }

            foreach (Vector2 tile in BattleManager.Field.TileDict.Keys)
            {
                if (!nonAttackTiles.Contains(tile))
                {
                    TileAdd(tile);
                }
            }
        }

        if (caster.Team == Team.Enemy)
            BattleManager.Field.SetEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Enemy);
        else if (caster.Team == Team.Player)
            BattleManager.Field.SetEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Friendly);
    }

    private void TileAdd(Vector2 coord)
    {
        _attackTile.Add(coord);
    }

    public override void ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) 
    {
        if (activeTiming == ActiveTiming.TURN_START)
        {
            NimrodFaceCheck(caster);
            SetAttackTile(caster);
        }
        else if (activeTiming == ActiveTiming.FIELD_UNIT_SUMMON)
        {
            SetAttackTile(caster);
        }
        else if (activeTiming == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (_nimrodFace == 1)
            {
                SetAttackTile(caster);
            }
        }
    }
    /*
    public virtual void ActionStart(BattleUnit attackUnit, List<BattleUnit> hits) => BattleManager.Instance.AttackStart(attackUnit, hits);
    public virtual void Action(BattleUnit attackUnit, BattleUnit receiver) => attackUnit.Attack(receiver, attackUnit.BattleUnitTotalStat.ATK);
    */
}
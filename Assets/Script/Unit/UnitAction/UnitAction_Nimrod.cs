using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Nimrod : UnitAction
{
    //0 index is vary last face, 1 index is second last face
    private int[] _recentState = {-1, -1};
    //0 = smile, 1 = weep, 2 = mad
    private int _nimrodState = 0;
    private List<Vector2> _attackTile = new();
    private Nimrod_Animation _nimrod_Animation = null;

    public override void AIMove(BattleUnit attackUnit)
    {
        BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
    }
     
    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
        {
            BattleManager.Instance.DirectAttack(attackUnit);
        }

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

        TileClear(attackUnit.Team);
    }

    private void NimrodStateCheck(BattleUnit caster)
    {
        if (_recentState[0] == -1)
        {
            _nimrodState = 0;
        }
        else 
        {
            int count = 0;

            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
                {
                    count++;
                }
            }

            if (count >= 6)
            {
                _nimrodState = 2;
            }
            else if (count == 0)
            {
                _nimrodState = 0;
            }
            else if (_recentState[0] == _recentState[1])
            {
                _nimrodState = _recentState[0] == 0 ? 1 : 0;
            }
            else
            {
                _nimrodState = Random.Range(0, 2);
            }
        }

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
            {
                unit.AnimatorSetBool("isBright", _nimrodState == 2);
            }
        }

        _recentState[0] = _nimrodState;
        _recentState[1] = _recentState[0];
    }

    private void SetAttackTile(BattleUnit caster)
    {
        TileClear(caster.Team);
        if (_nimrodState == 0)
        {
            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if (unit.Team != caster.Team)
                {
                    TileAdd(unit.Location);
                }
            }
        }
        else if (_nimrodState == 1)
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
                        if (udrlUnit != null && (udrlUnit.Data.ID == "오벨리스크ID" && udrlUnit.Team == caster.Team) || udrlUnit == caster)
                        {
                            continue;
                        }

                        TileAdd(unit.Location + udrl);
                    }
                }
            }
        }
        else if (_nimrodState == 2)
        {
            List<Vector2> nonAttackTiles = new();

            foreach (ConnectedUnit unit in caster.ConnectedUnits)
            {
                nonAttackTiles.Add(unit.Location);
            }
            nonAttackTiles.Add(caster.Location);

            foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
            {
                if ((unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team))
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
        {
            BattleManager.Field.SetEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Enemy);
        }
        else if (caster.Team == Team.Player)
        {
            BattleManager.Field.SetEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Friendly);
        }
    }

    private void TileAdd(Vector2 coord)
    {
        _attackTile.Add(coord);
    }

    private void TileClear(Team team)
    {
        _attackTile.Clear();

        if (team == Team.Enemy)
            BattleManager.Field.ClearEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Enemy);
        else if (team == Team.Player)
            BattleManager.Field.ClearEffectTile(_attackTile, EffectTileType.Nimrod_Attack_Friendly);
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) 
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            if (_nimrod_Animation == null)
            {
                _nimrod_Animation = GameManager.Resource.Instantiate("BattleUnits/Nimrod_Animtion").GetComponent<Nimrod_Animation>();
                _nimrod_Animation.ChangeAnimator(caster.Team);
            }
        }
        else if ((activeTiming & ActiveTiming.TURN_START) == ActiveTiming.TURN_START)
        {
            NimrodStateCheck(caster);
            SetAttackTile(caster);
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_SUMMON) == ActiveTiming.FIELD_UNIT_SUMMON)
        {
            if (_nimrodState == 0)
            {
                SetAttackTile(caster);
            }
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_DEAD) == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (_nimrodState == 1)
            {
                SetAttackTile(caster);
            }
        }
        else if ((activeTiming & ActiveTiming.AFTER_UNIT_DEAD) == ActiveTiming.AFTER_UNIT_DEAD || (activeTiming & ActiveTiming.FALLED) == ActiveTiming.FALLED)
        {
            int listCount = BattleManager.Data.BattleUnitList.Count;
            for (int i = 0; i < listCount; i++)
            {
                BattleUnit unit = BattleManager.Data.BattleUnitList[i];
                if (unit.Data.ID == "오벨리스크ID" && unit.Team == caster.Team)
                {
                    unit.UnitDiedEvent();
                    i--;
                    listCount--;
                }
            }
        }
        else if ((activeTiming & ActiveTiming.BEFORE_ATTACK) == ActiveTiming.BEFORE_ATTACK)
        {
            _nimrod_Animation.SetBool("isAttack", true);
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_START) == ActiveTiming.ATTACK_TURN_START)
        {
            foreach (Vector2 tile in _attackTile)
            {
                BattleManager.Field.TileDict[tile].IsColored = true;
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            _nimrod_Animation.SetBool("isAttack", false);
        }

        return false;
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (!_attackTile.Contains(coord))
            return false;

        List<BattleUnit> targetUnits = new();
        List<Vector2> emptyTiles = new();

        foreach (Vector2 tile in _attackTile)
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

        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/오벨리스크");
        sd.team = attackUnit.Team;


        if (hits.Count == 0)
        {
            sd.location = coord;
            BattleManager.Spawner.SpawnDataSpawn(sd);
        }
        else if (emptyTiles.Count > 0)
        {
            sd.location = emptyTiles[Random.Range(0, emptyTiles.Count)];
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

        TileClear(attackUnit.Team);

        return true;
    }
}
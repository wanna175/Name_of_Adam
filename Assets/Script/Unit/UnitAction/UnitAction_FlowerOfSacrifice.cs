using System.Collections.Generic;
using UnityEngine;

public class UnitAction_FlowerOfSacrifice : UnitAction
{
    private int _turnCount = 2;
    private BattleUnit _parentUnit = null;
    private UnitDataSO _spawnUnitSO = null;
    private Dictionary<int, string> _intToUnitName = new()
        {
            {1, "구원자의_검병"},
            {2, "구원자의_궁병" },
            {3, "구원자의_수녀" }
        };

    public override void AISkillUse(BattleUnit attackUnit)
    {
        BattleManager.Instance.EndUnitAction();
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 0)
            return false;

        BattleManager.Instance.EndUnitAction();

        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if (activeTiming == ActiveTiming.TURN_START)
        {
            _turnCount--;

            if (_turnCount <= 0)
            {
                UnitSpawn(caster);
            }
        }

        return false;
    }

    public override void SetValue(string sender, int value) 
    {
        if (sender == "Savior")
        {
            _spawnUnitSO = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/" + _intToUnitName[value]);
        }
    }

    public override void SetUnit(string sender, BattleUnit unit)
    {
        if (sender == "Savior")
        {
            _parentUnit = unit;
        }
    }

    private void UnitSpawn(BattleUnit caster)
    {
        BattleManager.Field.TileDict[caster.Location].ExitTile();

        if (_parentUnit == null)
        {
            _parentUnit = BattleManager.Data.BattleUnitList.Find(x => x.Data.ID == "구원자" && x.Team == caster.Team);
        }

        SpawnData sd = new();
        if (_spawnUnitSO == null)
        {
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/" + _intToUnitName[Random.Range(1, 4)]);
        }
        else
        {
            sd.unitData = _spawnUnitSO;
        }
        sd.location = caster.Location;
        sd.team = caster.Team;

        List<BattleUnit> arroundUnits = BattleManager.Field.GetArroundUnits(caster.Location, caster.Team == Team.Player ? Team.Enemy : Team.Player);
        foreach (BattleUnit arroundUnit in arroundUnits)
        {
            arroundUnit.ChangeFall(1);
        }

        foreach (BattleUnit arroundUnit in arroundUnits)
        {
            if (arroundUnit.Team != caster.Team)
            {
                arroundUnit.GetAttack(-(_parentUnit.BattleUnitTotalStat.ATK), caster);
            }
        }

        BattleUnit unit = BattleManager.Spawner.SpawnDataSpawn(sd);
        BattleManager.Data.BattleUnitList.Remove(caster);
        BattleManager.Data.BattleUnitRemoveFromOrder(caster);

        if (_parentUnit != null)
        {
            _parentUnit.Action.SetUnit("FlowerOfSacrifice", caster);
            _parentUnit.Action.SetUnit("FlowerOfSacrifice", unit);
        }

        unit.SetBuff(new Buff_Benediction());

        BattleManager.Spawner.RestoreUnit(caster.gameObject);
    }
}
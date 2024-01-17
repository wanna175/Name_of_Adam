using UnityEngine;
using System.Collections.Generic;

public class Stigma_Undertaker : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        if (caster.ConnectedUnits.Count == 2)
        {
            foreach (ConnectedUnit unit in caster.ConnectedUnits)
            {
                BattleManager.Instance.UnitDeadEvent(unit);
                BattleManager.Spawner.RestoreUnit(unit.gameObject);
            }

            caster.ConnectedUnits.Clear();
            caster.SetBuff(new Buff_Vice());

            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/ÁË¼ö");
            sd.location = caster.Location + Vector2.right;
            sd.team = caster.Team;

            BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Vice());

            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¹¦Áö±â");
            sd.location = caster.Location + Vector2.right + Vector2.right;
            sd.team = caster.Team;

            spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Vice());
        }
    }
}
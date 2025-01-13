using UnityEngine;

public class Stigma_Requiem : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Malevolence());

        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¿ø·É");
        sd.location = caster.Location + Vector2.right;
        sd.team = caster.Team;

        if (BattleManager.Field.GetUnit(sd.location) == null)
        {
            BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Malevolence());
        }

        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¸Á·É");
        sd.location = caster.Location + Vector2.right + Vector2.right;
        sd.team = caster.Team;

        if (BattleManager.Field.GetUnit(sd.location) == null)
        {
            BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Malevolence());
        }
        /*
        if (caster.ConnectedUnits.Count == 2)
        {
            foreach (ConnectedUnit unit in caster.ConnectedUnits)
            {
                BattleManager.Instance.UnitDeadEvent(unit);
                BattleManager.Spawner.RestoreUnit(unit.gameObject);
            }

            caster.ConnectedUnits.Clear();
            caster.SetBuff(new Buff_Malevolence());

            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¿ø·É");
            sd.location = caster.Location + Vector2.right;
            sd.team = caster.Team;

            BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Malevolence());

            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¸Á·É");
            sd.location = caster.Location + Vector2.right + Vector2.right;
            sd.team = caster.Team;

            spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
            spawnUnit.SetBuff(new Buff_Malevolence());

            caster.ResetHPBarPosition();
        }
        */
    }
}
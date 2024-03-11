using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerSkill_02_03 : PlayerSkill
{
    [SerializeField] private DeckUnit createUnit;

    public override bool Use(Vector2 coord)
    {
        GameManager.Sound.Play("UI/PlayerSkillSFX/Advent");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Advent", BattleManager.Field.GetTilePosition(coord));

        SpawnData spawnData = new();
        spawnData.unitData = createUnit.Data;
        spawnData.location = coord;
        spawnData.team = Team.Player;

        BattleManager.Spawner.SpawnDataSpawn(spawnData);
        BattleManager.Field.ClearAllColor();

        return false;
    }

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.Field.SetSpawnTileColor(FieldColorType.PlayerSkill, createUnit);
    }
}
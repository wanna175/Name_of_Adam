using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerSkill_02_03 : PlayerSkill
{
    [SerializeField]
    private DeckUnit createUnit;

    public override bool Use(Vector2 coord)
    {
        // GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        BattleManager.Spawner.DeckSpawn(createUnit, coord);
        GameManager.VisualEffect.StartVisualEffect(
            Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitSpawnBackEffect"),
            BattleManager.Field.GetTilePosition(coord) + new Vector3(0f, 3.5f, 0f));
        GameManager.VisualEffect.StartVisualEffect(
            Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitSpawnFrontEffect"),
            BattleManager.Field.GetTilePosition(coord) + new Vector3(0f, 3.5f, 0f));

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
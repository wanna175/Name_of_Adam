using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_01 : PlayerSkill
{
    private Vector2 _tilePos;

    public override bool Use(Vector2 coord)
    {
        _tilePos = coord;
        BattleUnit targetUnit = BattleManager.Field.GetUnit(_tilePos);
        targetUnit.GetAttack(-25, null);
        BattleManager.BattleCutScene.StartCoroutine(BattleManager.BattleCutScene.SkillHitEffect(targetUnit));

        GameManager.Sound.Play("UI/PlayerSkillSFX/DivinePunishment");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Punishment", BattleManager.Field.GetTilePosition(coord));
                
        return false;
    }

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.Enemy);
    }
}
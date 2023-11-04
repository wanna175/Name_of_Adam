using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Heal : PlayerSkill
{
    public override void Use(Vector2 coord)
    {
        BattleUnit targetUnit = BattleManager.Field.GetUnit(coord);
        GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        targetUnit.GetHeal(20, null);
    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.Friendly);

    }
}
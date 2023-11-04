using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Bless : PlayerSkill

{
    public override void Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));
        BattleManager.Field.GetUnit(coord).GetAttack(-30, null);
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

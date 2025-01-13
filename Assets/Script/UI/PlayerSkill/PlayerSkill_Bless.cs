using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Bless : PlayerSkill

{
    public override bool Use(Vector2 coord)
    {
        GameManager.Sound.Play("UI/PlayerSkillSFX/Prayer");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Prayer", BattleManager.Field.GetTilePosition(coord));

        BattleUnit unit = BattleManager.Field.GetUnit(coord);
        unit.SetBuff(new Buff_Curse());

        if (!GameManager.OutGameData.IsUnlockedItem(64))
        {
            unit.SetBuff(new Buff_AttackBoost());
            unit.SetBuff(new Buff_AttackBoost());
            unit.SetBuff(new Buff_AttackBoost());
        }

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

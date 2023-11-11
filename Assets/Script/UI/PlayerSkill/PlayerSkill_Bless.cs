using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Bless : PlayerSkill

{
    public override void Use(Vector2 coord, out bool isSkillOn)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        BattleUnit unit = BattleManager.Field.GetUnit(coord);
        unit.SetBuff(new Buff_Curse());
        unit.SetBuff(new Buff_Raise());
        isSkillOn = false;
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

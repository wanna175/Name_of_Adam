using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_02 : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        BattleUnit targetUnit = BattleManager.Field.GetUnit(coord);
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        int count = 3;
        if (true) // 혜원님 진척도 해방 시 조건을 여기에 작성해주세요.
            count = 5;

        for (int i = 0; i < count; i++)
            targetUnit.SetBuff(new Buff_Vice());
        targetUnit.ChangeFall(1);
        return false;
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
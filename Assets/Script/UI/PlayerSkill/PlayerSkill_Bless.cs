using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Bless : PlayerSkill

{
    public override bool Use(Vector2 coord)
    {
        GameManager.Sound.Play("UI/PlayerSkillSFX/Bless");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        BattleUnit unit = BattleManager.Field.GetUnit(coord);
        unit.SetBuff(new Buff_Curse());

        if (!GameManager.OutGameData.IsUnlockedItem(64))
        {
            unit.SetBuff(new Buff_Raise());
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

    public override string GetDescription()
    {
        SetDescription();
        return base.GetDescription();
    }

    public void SetDescription()
    {
        string description;

        if (GameManager.OutGameData.IsUnlockedItem(64))
        {
            description = "지정한 적에게 저주를 부여합니다";
        }
        else
        {
            description = "지정한 적에게 저주와 공격력 증가를 부여합니다";
        }

        base.SetDescription(description);
    }
}

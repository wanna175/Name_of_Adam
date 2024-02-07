using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_02 : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        BattleUnit targetUnit = BattleManager.Field.GetUnit(coord);
        GameManager.Sound.Play("UI/PlayerSkillSFX/SoulDeal");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        int count = 3;
        if (GameManager.OutGameData.IsUnlockedItem(73))
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

    public override string GetDescription()
    {
        SetDescription();
        return base.GetDescription();
    }

    public void SetDescription()
    {
        string description = "";

        switch (GameManager.Locale.CurrentLocaleIndex)
        {
            case 0: description = "Bestows malevolence 3 times and reduces faith by 1 to the designated ally."; break;
            case 1: description = "지정한 아군에게 악성을 3회 부여하고 신앙을 1 떨어뜨립니다."; break;
        }

        base.SetDescription(description);
    }
}
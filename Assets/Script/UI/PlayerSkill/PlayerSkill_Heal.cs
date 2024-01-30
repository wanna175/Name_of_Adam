using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Heal : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        BattleUnit targetUnit = BattleManager.Field.GetUnit(coord);
        GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));

        if (GameManager.OutGameData.IsUnlockedItem(62))
        {
            targetUnit.GetHeal(20, null);
        }
        else
        {
            targetUnit.GetHeal(15, null);
        }

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
        string description;

        if (GameManager.OutGameData.IsUnlockedItem(62))
        {
            description = "������ ���̳� �Ʊ��� ü���� 20 ȸ����ŵ�ϴ�";
        }
        else
        {
            description = "������ ���̳� �Ʊ��� ü���� 15 ȸ����ŵ�ϴ�";
        }

        base.SetDescription(description);
    }
}
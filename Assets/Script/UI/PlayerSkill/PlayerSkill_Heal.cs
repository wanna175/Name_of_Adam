using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Heal : PlayerSkill
{
    public override void Init()
    {
        base.playerSkillName = "Heal";
        base.manaCost = 20;
        base.darkEssence = 0;
        base.description = "20 마나를 지불하고 원하는 유닛의 체력을 20 회복합니다.";
    }
    public override void Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        BattleManager.Field.GetUnit(coord).ChangeHP(20);
    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.UnitTargetPlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.UnitTargetPlayerSkillReady(FieldColorType.PlayerSkill);
    }
}
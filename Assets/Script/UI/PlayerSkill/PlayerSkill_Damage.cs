using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Damage : PlayerSkill
{
    public override void Init()
    {
        base.playerSkillName = "Damage";
        base.manaCost = 20;
        base.darkEssence = 0;
        base.description = "20 마나를 지불하고 원하는 적 유닛에게 20 대미지를 줍니다.";
    }

    public override void Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        BattleManager.Field.GetUnit(coord).GetAttack(-20, null);
    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.EnemyTargetPlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.EnemyTargetPlayerSkillReady(FieldColorType.PlayerSkill);
    }
}
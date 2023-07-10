using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSkillHeal", menuName = "Scriptable Object/PlayerSkillHeal")]

public class PlayerSkill_Heal : PlayerSkill
{
    private string name = "Heal";
    private int manaCost = 20;
    private int darkEssence = 0;
    private string description = "20 마나를 지불하고 원하는 유닛의 체력을 20 회복합니다.";

    public override int GetDarkEssenceCost() => darkEssence;
    public override int GetManaCost() => manaCost;
    public override string GetName() => name;
    public override string GetDescription() => description;

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.UnitTargetPlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.UnitTargetPlayerSkillReady(FieldColorType.PlayerSkillHeal);
    }
}
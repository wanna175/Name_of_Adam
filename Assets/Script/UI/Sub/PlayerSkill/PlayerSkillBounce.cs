using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBounce : PlayerSkill
{
    private string name = "Bounce";
    private int manaCost = 20;
    private int darkEssence = 0;
    private string description = "20 마나를 지불하고 원하는 아군 유닛을 손으로 가져옵니다.";

    public override int GetDarkEssenceCost() => darkEssence;
    public override int GetManaCost() => manaCost;
    public override string GetName() => name;
    public override string GetDescription() => description;

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.FriendlyTargetPlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.FriendlyTargetPlayerSkillReady(FieldColorType.PlayerSkillBounce);
    }
}
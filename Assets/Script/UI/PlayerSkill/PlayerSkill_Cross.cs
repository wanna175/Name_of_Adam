using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Cross : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //ÀÌÆÑÆ®¸¦ ¿©±â¿¡ Ãß°¡
        List<Vector2> targetCoords = BattleManager.Field.GetCrossCoord(coord);
        GameManager.Sound.Play("UI/PlayerSkillSFX/Cross");

        // 좌표상 위에서부터 글 읽듯이 정렬
        targetCoords.Sort(delegate (Vector2 a, Vector2 b)
        {
            if (a.y > b.y)
                return 1;
            else if (a.y < b.y)
                return -1;
            else
            {
                if (a.x < b.x)
                    return 1;
                else
                    return -1;
            }
        });

        foreach (Vector2 target in targetCoords)
        {
            GameManager.VisualEffect.StartVisualEffect(
                "Arts/EffectAnimation/PlayerSkill/CrossThunder",
                BattleManager.Field.GetTilePosition(target) + new Vector3(0f, 4f, 0f));
            BattleUnit targetUnit = BattleManager.Field.GetUnit(target);

            if (targetUnit != null && targetUnit.Team == Team.Enemy)
            {
                BattleManager.BattleCutScene.StartCoroutine(BattleManager.BattleCutScene.SkillHitEffect(targetUnit));

                if (GameManager.OutGameData.IsUnlockedItem(54))
                {
                    targetUnit.ChangeFall(1);
                }

                if (!targetUnit.FallEvent)
                    targetUnit.GetAttack(-20, null);
            }
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
        string description = "";

        if (GameManager.OutGameData.IsUnlockedItem(54))
        {
            switch (GameManager.Locale.CurrentLocaleIndex)
            {
                case 0: description = "Deals 20 damage in a massive cross range near the designated enemy. 피격된 적의 신앙을 1 떨어뜨립니다."; break;
                case 1: description = "적을 지정하여 거대한 십자 범위에 20의 데미지를 줍니다. 피격된 적의 신앙을 1 떨어뜨립니다."; break;
            }    
        }
        else
        {
            switch (GameManager.Locale.CurrentLocaleIndex)
            {
                case 0: description = "Deals 20 damage in a massive cross range near the designated enemy."; break;
                case 1: description = "적을 지정하여 거대한 십자 범위에 20의 데미지를 줍니다."; break;
            }
        }

        base.SetDescription(description);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    BattleUIManager BattleUI;
    BattleDataManager Data;
    Field Field;
    PhaseController Phase;
    Mana Mana;

    void Awake()
    {
        BattleUI = BattleManager.BattleUI;
        Data = BattleManager.Data;
        Field = BattleManager.Field;
        Phase = BattleManager.Phase;
        Mana = BattleManager.Mana;
    }

    public void PlayerSkillUse()
    {
        PlayerSkill selectedSkill = BattleUI.GetSelectedPlayerSkill();

        Mana.ChangeMana(-1 * selectedSkill.GetManaCost());
        Data.DarkEssenseChage(-1 * selectedSkill.GetDarkEssenceCost());

        BattleUI.UI_playerSkill.CancelSelect();
        BattleUI.UI_playerSkill.Used = true;
        BattleUI.UI_playerSkill.InableSkill();
        Field.ClearAllColor();
    }

    public void PlayerSkillReady(FieldColorType colorType, PlayerSkillTargetType TargetType = PlayerSkillTargetType.none)
    {
        if (!Phase.CurrentPhaseCheck(Phase.Prepare))
            return;

        if (colorType == FieldColorType.none)
            Field.ClearAllColor();
        else
        { 
            if (TargetType == PlayerSkillTargetType.Unit)
                Field.SetUnitTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.Enemy)
                Field.SetEnemyUnitTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.Friendly)
                Field.SetFriendlyUnitTileColor(colorType);
        }
    }
}
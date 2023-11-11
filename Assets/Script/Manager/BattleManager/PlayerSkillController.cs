using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    BattleUIManager BattleUI;
    BattleDataManager Data;
    Field Field;
    PhaseController Phase;
    Mana Mana;

    public PlayerSkill usedPlayerSkill;
    public bool isSkillOn;

    void Awake()
    {
        BattleUI = BattleManager.BattleUI;
        Data = BattleManager.Data;
        Field = BattleManager.Field;
        Phase = BattleManager.Phase;
        Mana = BattleManager.Mana;
    }

    public void PlayerSkillUse(Vector2 coord)
    {
        Field.ClearAllColor();
        usedPlayerSkill = BattleUI.GetSelectedPlayerSkill();
        usedPlayerSkill.Use(coord, out isSkillOn);

        Mana.ChangeMana(-1 * usedPlayerSkill.GetManaCost());
        Data.DarkEssenseChage(-1 * usedPlayerSkill.GetDarkEssenceCost());
        if (!isSkillOn)
            usedPlayerSkill = null;

        BattleUI.UI_playerSkill.CancelSelect();
        BattleUI.UI_playerSkill.Used = true;
        BattleUI.UI_playerSkill.InableSkill();
    }

    public void SetSkillDone()
    {
        isSkillOn = false;
        usedPlayerSkill = null;
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
            else if (TargetType == PlayerSkillTargetType.all)
                Field.SetAllTileColor(colorType);
        }
    }
}
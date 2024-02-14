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

    PlayerSkill usedPlayerSkill;

    private bool _isSkillOn;
    public bool IsSkillOn => _isSkillOn;

    private bool _isManaFree;
    public bool IsManaFree => _isManaFree;

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
        _isSkillOn = usedPlayerSkill.Use(coord);

        if (_isManaFree)
        {
            _isManaFree = false;
            BattleUI.UI_playerSkill.RefreshSkill(GameManager.Data.GetPlayerSkillList());
        }
        else
        {
            Mana.ChangeMana(-1 * usedPlayerSkill.GetManaCost());
        }

        Data.DarkEssenseChage(-1 * usedPlayerSkill.GetDarkEssenceCost());

        if (!_isSkillOn)
            usedPlayerSkill = null;

        BattleUI.UI_playerSkill.CancelSelect();
        BattleUI.UI_playerSkill.Used = true;
        BattleUI.UI_playerSkill.InableSkill();

        BattleManager.Instance.BattleOverCheck();
    }

    public void ActionSkill(ActiveTiming activeTiming, Vector2 coord)
    {
        _isSkillOn = usedPlayerSkill.Action(activeTiming, coord);
    }

    public void SetSkillDone()
    {
        _isSkillOn = false;
        usedPlayerSkill = null;
    }

    public void SetManaFree(bool isActive) => _isManaFree = isActive;

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
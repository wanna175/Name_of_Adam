using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerSkill _usedPlayerSkill;

    private bool _isSkillOn;
    public bool IsSkillOn => _isSkillOn;

    private bool _isManaFree;
    public bool IsManaFree => _isManaFree;

    public void PlayerSkillUse(Vector2 coord)
    {
        BattleManager.Field.ClearAllColor();
        _usedPlayerSkill = BattleManager.BattleUI.GetSelectedPlayerSkill();
        _isSkillOn = _usedPlayerSkill.Use(coord);

        if (_isManaFree)
        {
            _isManaFree = false;
            BattleManager.BattleUI.UI_playerSkill.RefreshSkill(GameManager.Data.GetPlayerSkillList());
        }
        else
        {
            BattleManager.Mana.ChangeMana(-1 * _usedPlayerSkill.GetManaCost());
        }

        BattleManager.Data.DarkEssenseChage(-1 * _usedPlayerSkill.GetDarkEssenceCost());

        if (!_isSkillOn)
            _usedPlayerSkill = null;

        BattleManager.BattleUI.UI_playerSkill.CancelSelect();
        BattleManager.BattleUI.UI_playerSkill.InableSkill(true);

        BattleManager.Instance.BattleOverCheck();
    }

    public void ActionSkill(ActiveTiming activeTiming, Vector2 coord)
    {
        _isSkillOn = _usedPlayerSkill.Action(activeTiming, coord);
    }

    public void SetSkillDone()
    {
        _isSkillOn = false;
        _usedPlayerSkill = null;
    }

    public void SetManaFree(bool isActive) => _isManaFree = isActive;

    public void PlayerSkillReady(FieldColorType colorType, PlayerSkillTargetType TargetType = PlayerSkillTargetType.none)
    {
        if (!BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
            return;

        if (colorType == FieldColorType.none)
            BattleManager.Field.ClearAllColor();
        else
        {
            if (TargetType == PlayerSkillTargetType.Unit)
                BattleManager.Field.SetUnitTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.Enemy)
                BattleManager.Field.SetEnemyUnitTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.Friendly)
                BattleManager.Field.SetFriendlyUnitTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.all)
                BattleManager.Field.SetAllTileColor(colorType);
            else if (TargetType == PlayerSkillTargetType.NotBattleOnly)
                BattleManager.Field.SetNotBattleOnlyUnitTileColor(colorType);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PlayerSkillControler : MonoBehaviour
{
    BattleManager battle = BattleManager.Instance;
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
        PlayerSkill selectedSkill = BattleUI.UI_playerSkill.GetSelectedCard().GetSkill();

        Mana.ChangeMana(-1 * selectedSkill.GetManaCost());
        Data.DarkEssenseChage(-1 * selectedSkill.GetDarkEssenceCost());

        BattleUI.UI_playerSkill.CancelSelect();
        BattleUI.UI_playerSkill.Used = true;
        Field.ClearAllColor();
        battle.BattleOverCheck();
    }

    public void FallUnitOnField(Vector2 coord)
    {
        GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        Field.GetUnit(coord).ChangeFall(1);
        PlayerSkillUse();
    }

    public void DamageUnitOnField(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        Field.GetUnit(coord).ChangeHP(-20);
    }

    public void HealUnitOnField(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        Field.GetUnit(coord).ChangeHP(20);
    }

    public void BounceUnitOnField(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가

        BattleUnit unit = Field.GetUnit(coord);

        Data.BattleUnitRemove(unit);
        Data.BattleOrderRemove(unit);
        Data.AddDeckUnit(unit.DeckUnit);
        BattleUI.FillHand();
        Field.FieldCloseInfo(Field.TileDict[coord]);
        Destroy(unit.gameObject);
    }

    public bool UnitTargetPlayerSkillReady(FieldColorType colorType)
    {
        if (Phase.Current != Phase.Prepare)
            return false;

        if (colorType == FieldColorType.none)
            Field.ClearAllColor();
        else
            Field.SetUnitTileColor();

        battle.fieldColorType = colorType;

        return true;
    }

    public bool EnemyTargetPlayerSkillReady(FieldColorType colorType)
    {
        if (Phase.Current != Phase.Prepare)
            return false;

        if (colorType == FieldColorType.none)
            Field.ClearAllColor();
        else
            Field.SetEnemyUnitTileColor();

        battle.fieldColorType = colorType;

        return true;
    }

    public bool FriendlyTargetPlayerSkillReady(FieldColorType colorType)
    {
        if (Phase.Current != Phase.Prepare)
            return false;

        if (colorType == FieldColorType.none)
            Field.ClearAllColor();
        else
            Field.SetFriendlyUnitTileColor();

        battle.fieldColorType = colorType;

        return true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    [SerializeField] private int originalmanaCost;
    [SerializeField] private int originaldarkEssence;

    [SerializeField] private string playerSkillName;
    [SerializeField] private int playerSkillID;
    [SerializeField] private int manaCost;
    [SerializeField] private int darkEssence;
    [SerializeField] private Sprite skillImage;

    public int GetDarkEssenceCost() => darkEssence;
    public int GetManaCost()
    {
        if (BattleManager.PlayerSkillController.IsManaFree)
            return 0;
        else
            return manaCost;
    }

    public void ChangeCost(int mana, int darkessence)
    {
        manaCost = mana;
        darkEssence = darkessence;
    }

    public int GetOriginalManaCost() => originalmanaCost;
    public int GetOriginalDarkEssenceCost() => originaldarkEssence;
    public string GetName() => GameManager.Locale.GetLocalizedPlayerSkillName(playerSkillName);
    public int GetID() => playerSkillID;
    public virtual string GetDescription() => GameManager.Locale.GetLocalizedPlayerSkillInfo(playerSkillID);
    public Sprite GetSkillImage() => skillImage;

    public abstract bool Use(Vector2 coord);
    public virtual bool Action(ActiveTiming activeTiming, Vector2 coord) => false;
    public abstract void CancelSelect();
    public abstract void OnSelect();
}
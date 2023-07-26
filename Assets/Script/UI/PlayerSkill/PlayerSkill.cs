using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    protected string playerSkillName;
    protected int manaCost;
    protected int darkEssence;
    protected string description;

    public abstract void Init();
    public int GetDarkEssenceCost() => darkEssence;
    public int GetManaCost() => manaCost;
    public string GetName() => playerSkillName;
    public string GetDescription() => description;

    public abstract void Use(Vector2 coord);
    public abstract void CancelSelect();
    public abstract void OnSelect();
}
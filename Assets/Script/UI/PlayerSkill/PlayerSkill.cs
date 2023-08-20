using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    [SerializeField] private string playerSkillName;
    [SerializeField] private int manaCost;
    [SerializeField] private int darkEssence;
    [SerializeField] private string description;
    [SerializeField] private Sprite skillImage;

    public int GetDarkEssenceCost() => darkEssence;
    public int GetManaCost() => manaCost;
    public string GetName() => playerSkillName;
    public string GetDescription() => description;
    public Sprite GetSkillImage() => skillImage;

    public abstract void Use(Vector2 coord);
    public abstract void CancelSelect();
    public abstract void OnSelect();
}
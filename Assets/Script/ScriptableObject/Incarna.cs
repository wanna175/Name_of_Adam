using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Incarna", menuName = "Scriptable Object/Incarna")]

public class Incarna : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public Sprite Sprite;
    [SerializeField] public List<PlayerSkill> PlayerSkillList;
    [SerializeField] public int PlayerSkillCount;
}
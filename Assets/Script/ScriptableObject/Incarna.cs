using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Incarna", menuName = "Scriptable Object/Incarna")]

[Serializable]
public class Incarna : ScriptableObject
{
    [SerializeField] private string _id;
    public string ID => _id;

    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private List<PlayerSkill> _playerSkillList = new();
    public List<PlayerSkill> PlayerSkillList => _playerSkillList;
}
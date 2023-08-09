using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Incarna", menuName = "Scriptable Object/Incarna")]

public class Incarna : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private List<PlayerSkill> _playerSkillList = new();
    public List<PlayerSkill> PlayerSkillList => _playerSkillList;

    [SerializeField] private int _playerSkillCount;
    public int PlayerSkillCount => _playerSkillCount;
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passive : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private PassiveType _passvieType;
    public PassiveType PassiveType => _passvieType;

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity => _rarity;

    [SerializeField, TextArea] private string _description;
    public string Description => _description;

    [SerializeField] private bool _isSpecial = false;
    public bool IsSpecial => _isSpecial;

    // Memo : 사실 윗 부분만 있다면 SO로 떼서 관리하는 편이 나음...
    // Use의 내용이 달라서 Prefab으로 관리하는 건데 이를 개선할 방법은 없을까

    public virtual void Use(BattleUnit caster, BattleUnit receiver)
    {
        return;
    }
}
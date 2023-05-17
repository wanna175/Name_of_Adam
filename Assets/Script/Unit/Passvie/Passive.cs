using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum 낙인
{
    고양, 자애, 강림, // 소환 시
    가학, 흡수, 처형, 대죄, // 공격 후
    오빠, 동생, 고문관, 망령 // 특수 낙인
}

public class Passive : MonoBehaviour
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private PassiveType _passvieType;
    public PassiveType PassiveType => _passvieType;

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity => _rarity;

    [SerializeField, TextArea] private string _description;
    public string Description => _description;

    [SerializeField] private bool _isSpecial = false;
    public bool IsSpecial => _isSpecial;

    public virtual void Use(BattleUnit caster, BattleUnit receiver)
    {
        return;
    }
}
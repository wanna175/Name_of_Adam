using System;
using System.Collections;
using UnityEngine;


public enum 낙인
{
    고양, 자애, 강림, // 소환 시
    가학, 흡수, 처형, 대죄, // 공격 후
}

public class Passive
{
    private PassiveType type;
    public PassiveType PassiveType => type;

    public void Use(BattleUnit caster, BattleUnit receiver)
    {

    }
}


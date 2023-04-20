using System;
using System.Collections;
using UnityEngine;


public enum 낙인
{
    고양, 자애, 강림, // 소환 시
    가학, 흡수, 처형, 대죄, // 공격 후
}

public abstract class Passive
{
    private PassiveType type;
    public PassiveType PassiveType => type;

    public abstract PassiveType GetPassiveType();

    public abstract void Use(BattleUnit caster, BattleUnit receiver);
}

public class 가학 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        Debug.Log($"{caster.name} 가학");
        caster.ChangedStat.ATK += 3;
    }
}

public class 흡수 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        throw new NotImplementedException();
    }
}

public class 처형 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        throw new NotImplementedException();
    }
}

public class 대죄 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        throw new NotImplementedException();
    }
}
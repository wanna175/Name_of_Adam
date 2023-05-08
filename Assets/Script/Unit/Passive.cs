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

public abstract class Passive
{
    private PassiveType type;
    public PassiveType PassiveType => type;

    public abstract PassiveType GetPassiveType();

    public abstract void Use(BattleUnit caster, BattleUnit receiver);
}

public class 가학 : Passive
{
    private bool isApplied = false;

    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (isApplied)
            return;

        caster.ChangedStat.ATK += 3;
        isApplied = true;
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
        double heal = caster.Stat.ATK * 0.3;
        caster.ChangeHP(((int)heal));
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
        if (receiver.HP.GetCurrentHP() <= 10)
        {
            receiver.ChangeHP(-receiver.HP.GetCurrentHP());
        }
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
        receiver.ChangeFall(1);
    }
}

public class 고양 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);

        List<BattleUnit> targetUnits = BattleManager.Instance.GetArroundUnits(targetCoords);

        foreach(BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
                unit.ChangedStat.ATK += 5;
        }
    }
}

public class 자애 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);
        targetCoords.Add(caster.Location + new Vector2(-1, -1));
        targetCoords.Add(caster.Location + new Vector2(-1, 1));
        targetCoords.Add(caster.Location + new Vector2(1, 1));
        targetCoords.Add(caster.Location + new Vector2(1, -1));

        List<BattleUnit> targetUnits = BattleManager.Instance.GetArroundUnits(targetCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
                unit.ChangeHP(20);
        }
    }
}

public class 강림 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.SUMMON;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> targetCoords = new List<Vector2>();
        targetCoords.Add(caster.Location + Vector2.up);
        targetCoords.Add(caster.Location + Vector2.down);
        targetCoords.Add(caster.Location + Vector2.right);
        targetCoords.Add(caster.Location + Vector2.left);

        List<BattleUnit> targetUnits = BattleManager.Instance.GetArroundUnits(targetCoords);

        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team != caster.Team)
                unit.ChangeHP(-15);
        }
    }
}

public class 오빠 : Passive
{
    private 동생 _connectedPassive; // 연결된 동생 낙인
    private List<BattleUnit> _pointedUnits = new List<BattleUnit>(); // 해의 증표 붙은 애들

    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (_connectedPassive == null)
            FindConnectPassive();

        if (_connectedPassive == null)
            return;

        List<BattleUnit> units = _connectedPassive.GetPointedUnits();
        if (units.Contains(receiver))
        {
            receiver.ChangeFall(1);
            caster.ChangeHP(10);
            _connectedPassive.RemoveMoonMark(receiver);
        }

        AddSunMark(receiver);
    }

    // 필드 위에 동생 낙인 있나 확인
    private void FindConnectPassive()
    {
        foreach(BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.DeckUnit.Data.Name.Equals("남매-여동생") == false)
                continue;

            foreach (Passive passive in unit.DeckUnit.Stigmata)
                if (passive.GetType() == typeof(동생))
                {
                    _connectedPassive = passive as 동생;
                    Debug.Log("오빠 낙인 등록");
                    return;
                }
        }
    }

    public List<BattleUnit> GetPointedUnits()
    {
        return _pointedUnits;
    }

    public void RemoveSunMark(BattleUnit unit)
    {
        _pointedUnits.Remove(unit);
    }

    public void AddSunMark(BattleUnit unit)
    {
        Debug.Log($"{unit.Data.Name}에 해의 증표");
        _pointedUnits.Add(unit);
    }
}

public class 동생 : Passive
{
    private 오빠 _connectedPassive; // 연결된 오빠 낙인
    private List<BattleUnit> _pointedUnits = new List<BattleUnit>(); // 달의 증표가 붙은 애들

    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        if (_connectedPassive == null)
            FindConnectPassive();

        if (_connectedPassive == null)
            return;
           
        List<BattleUnit> units = _connectedPassive.GetPointedUnits();

        foreach(BattleUnit unit in units)
        {
            unit.ChangeHP(-caster.Stat.ATK);
            unit.ChangeFall(1);
            AddMoonMark(unit);
        }
        _connectedPassive.GetPointedUnits().Clear();
    }

    private void FindConnectPassive()
    {
        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            // 여기 이름 남매-오빠로 바꿔야 함
            if (unit.DeckUnit.Data.Name.Equals("검병") == false) 
                continue;

            foreach (Passive passive in unit.DeckUnit.Stigmata)
                if (passive.GetType() == typeof(오빠))
                {
                    _connectedPassive = passive as 오빠;
                    Debug.Log("동생 낙인 등록");
                    return;
                }     
        }
    }

    public List<BattleUnit> GetPointedUnits()
    {
        return _pointedUnits;
    }

    public void RemoveMoonMark(BattleUnit unit)
    {
        _pointedUnits.Remove(unit);
    }

    public void AddMoonMark(BattleUnit unit)
    {
        Debug.Log($"{unit.Data.Name}에 달의 증표");
        _pointedUnits.Add(unit);
    }
}

public class 고문관 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        Vector2 moveVec = (receiver.Location - caster.Location).normalized;

        if (!BattleManager.Field.TileDict[caster.Location + moveVec].UnitExist)
        {
            Debug.Log(caster.Location + moveVec);
            BattleManager.Field.MoveUnit(receiver.Location, caster.Location +  moveVec);
        }
    }
}

public class 망령 : Passive
{
    public override PassiveType GetPassiveType()
    {
        return PassiveType.AFTERATTACK;
    }

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        Vector2 moveVec = receiver.Location + (receiver.Location - caster.Location);

        if (!BattleManager.Field.TileDict.ContainsKey(moveVec))
            return;

        if (!BattleManager.Field.TileDict[moveVec].UnitExist)
            BattleManager.Field.MoveUnit(caster.Location, moveVec);
    }
}
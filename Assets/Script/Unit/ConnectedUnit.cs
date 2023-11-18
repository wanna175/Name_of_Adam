using System;
using UnityEngine;

public class ConnectedUnit : BattleUnit
{
    private BattleUnit _origianlUnit;

    public void SetOriginalUnit(BattleUnit origianlUnit) => _origianlUnit = origianlUnit;

    public override BattleUnit GetOriginalUnit() => _origianlUnit;

    public override void GetAttack(int value, BattleUnit caster) => _origianlUnit.GetAttack(value, caster);

    public override void GetHeal(int value, BattleUnit caster) => _origianlUnit.GetHeal(value, caster);

    public override void SetBuff(Buff buff) => _origianlUnit.SetBuff(buff);

    public override int GetHP() => _origianlUnit.GetHP();
}
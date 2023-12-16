using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Berserker : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Berserker;

        _name = "광화";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "매 턴 공격력 50% 증가 합니다.";

        _count = 1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        return false;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK = (int)(_owner.DeckUnit.DeckUnitTotalStat.ATK * _count * 0.5);

        return stat;
    }
}

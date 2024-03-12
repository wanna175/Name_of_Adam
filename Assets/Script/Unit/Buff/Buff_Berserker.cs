using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Berserker : Buff
{
    private int attackUp;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Berserker;

        _name = "Attack Increase";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "Attack Increase Info";

        _count = 1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.ATTACK_MOTION_END;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = true;

        attackUp = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override bool Active(BattleUnit caster)
    {
        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += attackUp;

        return stat;
    }
}

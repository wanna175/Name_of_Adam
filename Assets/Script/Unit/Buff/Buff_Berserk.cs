using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Berserk : Buff
{
    private int _attackUp;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Berserk;

        _name = "Attack Boost";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_AttackBoost_Sprite");

        _description = "Attack Boost Info";

        _owner = owner;

        _statBuff = true;

        _dispellable = true;

        _attackUp = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += _attackUp;

        return stat;
    }
}

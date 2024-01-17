using System.Collections.Generic;
using UnityEngine;

public class Buff_Karma : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Karma;

        _name = "업보";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "업보.";

        _count = 2;

        _countDownTiming = ActiveTiming.AFTER_ATTACKED;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACKED;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        caster.ChangeFall(1);

        return false;
    }
}
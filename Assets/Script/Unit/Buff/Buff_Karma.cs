using System.Collections.Generic;
using UnityEngine;

public class Buff_Karma : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Karma;

        _name = "Karma";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Malevolence_Sprite");

        _description = "Karma Info";

        _count = 3;

        _countDownTiming = ActiveTiming.AFTER_ATTACKED;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACKED;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}
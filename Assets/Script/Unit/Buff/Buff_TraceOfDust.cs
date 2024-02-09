using System.Collections.Generic;
using UnityEngine;

public class Buff_TraceOfDust: Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.TraceOfDust;

        _name = "Dusk";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_TraceOfLunar_Sprite");

        _description = "Dust Info";

        _count = 1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
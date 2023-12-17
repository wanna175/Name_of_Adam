using System.Collections.Generic;
using UnityEngine;

public class Buff_TraceOfDust: Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.TraceOfDust;

        _name = "황혼";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_TraceOfLunar_Sprite");

        _description = "1중첩: 엘리우스, 야나에게 피격시 주변 4칸의 아군도 피격됩니다.\n2중첩: 엘리우스, 야나에게 피격시 신앙이 떨어집니다.";

        _count = 1;

        _countDownTiming = ActiveTiming.AFTER_ATTACK;

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
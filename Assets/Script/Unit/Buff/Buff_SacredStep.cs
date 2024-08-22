using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_SacredStep : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.SacredStep;

        _name = "Sacred Step";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_SacredStep_Sprite");

        _description = "Sacred Step Info";

        _count = 1;

        _countDownTiming = ActiveTiming.MOVE_TURN_END;

        _owner = owner;

        _dispellable = true;
    }

    public override void Stack()
    {
        _count++;
    }

    public override bool Active(BattleUnit caster)
    {
        // BattleUnit._isTeleportOn으로 대체

        return false;
    }
}

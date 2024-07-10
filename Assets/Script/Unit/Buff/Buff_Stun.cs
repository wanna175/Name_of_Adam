using UnityEngine;

public class Buff_Stun : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stun;

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Stun_Sprite");

        _name = "Stun";

        _description = "Stun Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START | ActiveTiming.ATTACK_TURN_START;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        return true;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
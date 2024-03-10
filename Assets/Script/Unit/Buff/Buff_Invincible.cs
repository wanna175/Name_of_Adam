using UnityEngine;

public class Buff_Invincible : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Invincible;

        _name = "Invincibility";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Invincible_Sprite");

        _description = "Invincibility Info";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACKED;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

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
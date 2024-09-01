using UnityEngine;

public class Buff_Invincibility : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Invincibility;

        _name = "Invincibility";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Invincibility_Sprite");

        _description = "Invincibility Info";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACKED;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _owner = owner;

        _dispellable = true;
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
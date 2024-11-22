using UnityEngine;

public class Buff_Bind : Buff
{    
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Bind;

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Bind_Sprite");

        _name = "Bind";

        _description = "Bind Info";

        _count = 1;

        _countDownTiming = ActiveTiming.MOVE_TURN_START;

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _isDebuff = true;
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
using UnityEngine;

public class Buff_Curse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Curse;

        _name = "Curse";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Curse_Sprite");

        _description = "Curse Info";

        _buffActiveTiming = ActiveTiming.TURN_START;

        _owner = owner;

        _isDebuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangeFall(1);

        return false;
    }
}
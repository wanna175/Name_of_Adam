using UnityEngine;

public class Buff_Immortality : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Immortality;

        _name = "Immortality";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Immortality_Sprite");

        _description = "Immortality Info";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_UNIT_DEAD;

        _buffActiveTiming = ActiveTiming.BEFORE_UNIT_DEAD;

        _owner = owner;

        _dispellable = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangeHP(-1 * _owner.HP.GetCurrentHP() + 1);
        return true;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
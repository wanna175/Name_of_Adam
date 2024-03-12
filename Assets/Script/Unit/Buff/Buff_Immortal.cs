using UnityEngine;

public class Buff_Immortal : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Immortal;

        _name = "Immortality";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Immortal_Sprite");

        _description = "Immortality Info";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_UNIT_DEAD;

        _buffActiveTiming = ActiveTiming.BEFORE_UNIT_DEAD;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
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
using UnityEngine;

public class Buff_Invincible : Buff
{    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Invincible;

        _name = "公利";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Invincible_Sprite");

        _description = "公利.";

        _count = 1;

        _countDownTiming = ActiveTiming.BEFORE_ATTACKED;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        return true;
    }

    public override void Stack()
    {
        _count += 1;
    }
}
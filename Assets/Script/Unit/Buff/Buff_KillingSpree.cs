using UnityEngine;

public class Buff_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.KillingSpree_Buff;

        _name = "Killing Spree";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_KillingSpree_Sprite");

        _description = "Killing Spree Info";

        _count = 2;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;
    }
}
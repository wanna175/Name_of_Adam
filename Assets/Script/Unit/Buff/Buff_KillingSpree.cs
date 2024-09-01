using UnityEngine;

public class Buff_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.KillingSpree;

        _name = "Killing Spree";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_KillingSpree_Sprite");

        _description = "Killing Spree Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        BattleManager.Data.BattleOrderInsert(0, _owner, _owner.BattleUnitTotalStat.SPD);
    }

    public override void Stack()
    {
        _count += 1;
    }
}
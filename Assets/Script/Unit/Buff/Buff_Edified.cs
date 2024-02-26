using UnityEngine;

public class Buff_Edified : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Edified;

        _name = "Edified";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "Edified";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }
}
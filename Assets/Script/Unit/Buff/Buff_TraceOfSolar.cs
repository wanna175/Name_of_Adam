using UnityEngine;

public class Buff_TraceOfSolar: Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.TraceOfSolar;

        _name = "태양의 흔적";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_TraceOfSolar_Sprite");

        _description = "달의 흔적이 부여될 시 신앙을 1 떨어뜨리고 사라집니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }
}
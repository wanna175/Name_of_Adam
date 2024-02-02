using UnityEngine;

public class Buff_MarkOfRaquel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.MarkOfRaquel;

        _name = "라헬의 표식";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Tailwind_Sprite");

        _description = "레아에게 공격받을 시 10 대미지를 입고 신앙이 1 떨어집니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }
}
using UnityEngine;

public class Buff_TraceOfLunar: Buff
{    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.TraceOfLunar;

        _name = "달의 흔적";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_TraceOfLunar_Sprite");

        _description = "태양의 흔적이 부여될 시 신앙을 1 떨어뜨리고 사라집니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }
}
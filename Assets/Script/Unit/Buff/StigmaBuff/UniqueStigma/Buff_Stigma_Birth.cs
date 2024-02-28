using UnityEngine;

public class Buff_Stigma_Birth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Birth;

        _name = "탄생";

        _description = "자신의 턴에 희생의 꽃을 소환합니다. \n희생의 꽃은 다음 구원자의 턴에 개화하며 주변 4칸의 적에게 구원자의 공격력만큼의 데미지를 줍니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
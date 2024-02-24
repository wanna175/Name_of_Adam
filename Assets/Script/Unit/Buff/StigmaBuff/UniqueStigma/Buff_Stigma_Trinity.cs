using UnityEngine;

public class Buff_Stigma_Trinity : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Trinity;

        _name = "������ü";

        _description = "�� �� ���⸦ �ٲ� ��Ÿ��� ȿ���� �޶����ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
using UnityEngine;

public class Buff_Stigma_PermanenceOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.PermanenceOfBabel;

        _name = "�ٺ��� ����";

        _description = "�� ������ �÷��̾��Ͽ� �ִ� Ÿ���� �����ϰų�, ��������ũ ������ �� ���ֵ��� �����մϴ�. \n�ٴ����� ������ Ÿ���� �÷��̾��Ϻ��� ǥ�õ˴ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
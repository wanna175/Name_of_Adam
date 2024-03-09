using UnityEngine;

public class Buff_Stigma_Birth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Birth;

        _name = "ź��";

        _description = "�ڽ��� �Ͽ� ����� ���� ��ȯ�մϴ�. \n����� ���� ���� �������� �Ͽ� ��ȭ�ϸ� �ֺ� 4ĭ�� ������ �������� ���ݷ¸�ŭ�� �������� �ݴϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}
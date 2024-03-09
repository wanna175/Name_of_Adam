using UnityEngine;

public class Buff_MarkOfBeast : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.MarkOfBeast;

        _name = "������ ����";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_MarkOfBeast_Sprite");

        _description = "����, ���ƿ��� ���� ������ �ž��� 1 �������ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }
}
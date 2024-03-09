using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Mercy : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Mercy;

        _name = "�ں�";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "���ݷ��� 0�� �Ǵ� ��� �ǰ� ����� �ž��� 1 ����߸��ϴ�";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangedDamage = 0;
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_ForbiddenPact : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.ForbiddenPact;

        _name = "������ ���";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "��ȯ �� �÷��̾� ��ų ���� ����� �������� �ʽ��ϴ�. ��� �ش� ������ ������ 1�� �ο��˴ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        BattleManager.PlayerSkillController.SetManaFree(true);
        BattleManager.BattleUI.UI_playerSkill.RefreshSkill(GameManager.Data.GetPlayerSkillList());
        _owner.SetBuff(new Buff_Stun());

        return false;
    }
}

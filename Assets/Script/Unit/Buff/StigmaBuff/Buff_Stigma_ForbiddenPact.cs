using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_ForbiddenPact : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.ForbiddenPact;

        _name = "금지된 계약";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "소환 시 플레이어 스킬 마나 비용을 지불하지 않습니다. 대신 해당 유닛은 스턴이 1턴 부여됩니다.";

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
        if (caster.Team == Team.Player)
        {
            BattleManager.PlayerSkillController.SetManaFree(true);
            BattleManager.BattleUI.UI_playerSkill.RefreshSkill(GameManager.Data.GetPlayerSkillList());
            _owner.SetBuff(new Buff_Stun());
        }

        return false;
    }
}

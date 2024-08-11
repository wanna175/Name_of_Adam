using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_ForbiddenPact : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_ForbiddenPact;

        _name = "ForbiddenPact";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (_owner.Team == Team.Player)
        {
            BattleManager.PlayerSkillController.SetManaFree(true);
            BattleManager.BattleUI.UI_playerSkill.RefreshSkill(GameManager.Data.GetPlayerSkillList());
            _owner.SetBuff(new Buff_AttackDecrease());
        }

        return false;
    }
}

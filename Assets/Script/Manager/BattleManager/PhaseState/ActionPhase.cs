using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActionPhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();
        _nowUnit.AttackTurnStart();

        BattleManager.Field.SetTileColor(_nowUnit, FieldColor.Attack);
        BattleManager.BattleUI.ChangeButtonName();

        if (BattleManager.Data.GetNowUnit().Team == Team.Enemy)
            BattleManager.Instance.PlayAfterCoroutine(_nowUnit.AI.AISkillUse, 1);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.ActionPhaseClick();
    }

    public override void OnStateExit()
    {
        _nowUnit.AttackTurnEnd();
        _nowUnit = null;
    }
}
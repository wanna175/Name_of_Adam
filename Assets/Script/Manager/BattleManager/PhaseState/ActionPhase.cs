using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActionPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleUnit unit = BattleManager.Data.GetNowUnit();

        BattleManager.Field.SetTileColor(unit, FieldColor.Attack);
        BattleManager.BattleUI.ChangeButtonName();

        if (BattleManager.Data.GetNowUnit().Team == Team.Enemy)
            BattleManager.Instance.PlayAfterCoroutine(unit.AI.AISkillUse, 1);

    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.ActionPhase();
    }

    public override void OnStateExit()
    {

    }
}
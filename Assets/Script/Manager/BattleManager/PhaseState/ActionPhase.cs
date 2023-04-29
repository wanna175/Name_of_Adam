using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActionPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Field.SetTileColor(BattleManager.Data.GetNowUnit(), ClickType.Attack);
        BattleManager.Instance.ChangeButtonName();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        BattleManager.Instance.ActionPhase();
    }

    public override void OnStateExit()
    {
        BattleManager.Field.ClearAllColor();
        BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnit());
        BattleManager.Instance.BattleOverCheck();
    }
}
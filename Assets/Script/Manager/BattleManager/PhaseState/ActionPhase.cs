using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActionPhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.Field.SetTileColor(_battle.Data.GetNowUnit(), ClickType.Attack);
        _battle.ChangeButtonName();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        _battle.ActionPhase();
    }

    public override void OnStateExit()
    {
        _battle.Field.ClearAllColor();
        _battle.Data.BattleOrderRemove(_battle.Data.GetNowUnit());
        _battle.BattleOverCheck();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MovePhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Field.SetTileColor(BattleManager.Data.GetNowUnit(), ClickType.Move);
        BattleManager.Instance.ChangeButtonName();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        BattleManager.Instance.MovePhase();
    }

    public override void OnStateExit()
    {
        BattleManager.Field.ClearAllColor();
    }
}

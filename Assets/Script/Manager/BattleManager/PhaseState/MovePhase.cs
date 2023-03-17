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
        _battle.Field.SetTileColor(_battle.Data.GetNowUnit(), ClickType.Move);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        _battle.MovePhase();
    }

    public override void OnStateExit()
    {
        _battle.Field.ClearAllColor();
    }
}

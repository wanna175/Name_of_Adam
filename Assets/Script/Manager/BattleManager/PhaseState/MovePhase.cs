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
        BattleUnit unit = BattleManager.Data.GetNowUnit();

        BattleManager.Field.SetTileColor(unit, FieldColor.Move);
        BattleManager.BattleUI.ChangeButtonName();

        if (BattleManager.Data.GetNowUnit().Team == Team.Enemy)
            BattleManager.Instance.PlayAfterCoroutine(unit.AI.AIMove, 1);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.MovePhase();
    }

    public override void OnStateExit()
    {
        BattleManager.Field.ClearAllColor();
    }
}

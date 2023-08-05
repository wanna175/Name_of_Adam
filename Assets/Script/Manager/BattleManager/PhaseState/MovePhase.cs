using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MovePhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();
        _nowUnit.MoveTurnStart();

        BattleManager.Field.SetTileColor(_nowUnit, FieldColor.Move);
        BattleManager.BattleUI.ChangeButtonName();

        if (_nowUnit.Team == Team.Enemy)
            BattleManager.Instance.PlayAfterCoroutine(_nowUnit.AI.AIMove, 1);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.MovePhaseClick();
    }

    public override void OnStateExit()
    {
        _nowUnit.MoveTurnStart();
        _nowUnit = null;

        BattleManager.Field.ClearAllColor();
    }
}

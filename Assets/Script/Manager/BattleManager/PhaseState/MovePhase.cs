using System;
using System.Collections;
using UnityEngine;

public class MovePhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();
        _nowUnit.MoveTurnStart();

        BattleManager.Field.SetNextActionTileColor(_nowUnit, FieldColorType.Move);

        if (_nowUnit.Team == Team.Enemy)
            BattleManager.Instance.StartCoroutine(NowUnitMove());
    }

    private IEnumerator NowUnitMove()
    {
        yield return new WaitForSeconds(1f);

        _nowUnit.Action.AIMove(_nowUnit);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent(Vector2 coord)
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.MovePhaseClick(coord);
    }

    public override void OnStateExit()
    {
        _nowUnit.MoveTurnStart();
        _nowUnit = null;

        BattleManager.Field.ClearAllColor();
    }
}

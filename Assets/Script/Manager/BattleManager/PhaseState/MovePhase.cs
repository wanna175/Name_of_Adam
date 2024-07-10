using System;
using System.Collections;
using UnityEngine;

public class MovePhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        if (BattleManager.Data.isGameDone)
            return;

        _nowUnit = BattleManager.Data.GetNowUnit();
        BattleManager.Field.SetTileHighlightFrame(_nowUnit.Location, true);

        if (_nowUnit.Team == Team.Player)
            BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(true);
        
        if (_nowUnit.Team == Team.Player && !GameManager.OutGameData.IsTutorialClear())
            TutorialManager.Instance.ShowNextTutorial();

        _nowUnit.NextMoveSkip = BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE_TURN_START, _nowUnit);

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
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE_TURN_END, _nowUnit);
        _nowUnit.NextMoveSkip = false;
        _nowUnit = null;

        BattleManager.Field.ClearAllColor();
    }
}

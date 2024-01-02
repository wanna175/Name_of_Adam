using System;
using System.Collections;
using UnityEngine;

public class MovePhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();

        Debug.Log($"{_nowUnit.Data.name} : {_nowUnit.Team}");
        if (_nowUnit.Team == Team.Player && !GameManager.OutGameData.isTutorialClear())
            TutorialManager.Instance.ShowNextTutorial();

        //�̵� �� ���� �� üũ
        _nowUnit.NextMoveSkip = BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE_TURN_START, _nowUnit);
        _nowUnit.NextMoveSkip |= BattleManager.Instance.ActiveTimingCheck(ActiveTiming.ACTION_TURN_START, _nowUnit);

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

using System;
using System.Collections;
using UnityEngine;

public class ActionPhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();

        //공격 턴 시작 시 체크
        _nowUnit.NextAttackSkip = BattleManager.Instance.ActiveTimingCheck(ActiveTiming.ATTACK_TURN_START, _nowUnit);
        _nowUnit.NextAttackSkip |= BattleManager.Instance.ActiveTimingCheck(ActiveTiming.ACTION_TURN_START, _nowUnit);

        BattleManager.Field.SetNextActionTileColor(_nowUnit, FieldColorType.Attack);

        if (_nowUnit.Team == Team.Enemy)
            BattleManager.Instance.StartCoroutine(NowUnitAction());
    }

    private IEnumerator NowUnitAction()
    {
        yield return new WaitForSeconds(1.5f);

        _nowUnit.Action.AISkillUse(_nowUnit);
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent(Vector2 coord)
    {
        if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            BattleManager.Instance.ActionPhaseClick(coord);
    }

    public override void OnStateExit()
    {
        BattleManager.Field.ClearAllColor();

        //공격 턴 종료 시 체크
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.ATTACK_TURN_END, _nowUnit);
        BattleManager.Instance.FieldActiveEventCheck(ActiveTiming.FIELD_ATTACK_TURN_END);
        
        _nowUnit.NextAttackSkip = false;
        _nowUnit.AttackUnitNum = 0;
        _nowUnit = null;
    }
}
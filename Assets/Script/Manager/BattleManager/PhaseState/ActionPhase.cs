using System;
using System.Collections;
using UnityEngine;

public class ActionPhase : Phase
{
    private BattleUnit _nowUnit;

    public override void OnStateEnter()
    {
        _nowUnit = BattleManager.Data.GetNowUnit();
        _nowUnit.AttackTurnStart();

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

        _nowUnit.AttackTurnEnd();
        _nowUnit = null;
    }
}
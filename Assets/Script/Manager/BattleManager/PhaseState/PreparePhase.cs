using UnityEngine;

public class PreparePhase : Phase
{
    public bool isFirst = true;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("Stage_Transition/Prepare/PrepareEnter");
        BattleManager.Mana.ChangeMana(15);
        BattleManager.Data.TurnPlus();
        BattleManager.BattleUI.UI_playerSkill.Used = false;
        BattleManager.BattleUI.ChangeButtonName();
        BattleManager.BattleUI.UI_turnNotify.SetPlayerTurn();
        BattleManager.Instance.TurnStart();
    }

    public override void OnStateUpdate()
    {
        BattleManager.Data.BattleUnitOrder();
    }

    public override void OnClickEvent(Vector2 coord)
    {
        if (isFirst)
        {
            BattleManager.Instance.StartPhaseClick(coord);
        }
        else
        {
            BattleManager.Instance.PreparePhaseClick(coord);
        }
    }

    public override void OnStateExit()
    {
        if (isFirst) 
        {
            foreach (DeckUnit unit in BattleManager.Data.PlayerDeck)
                unit.FirstTurnDiscountUndo();

            foreach (DeckUnit unit in BattleManager.Data.PlayerHands)
                unit.FirstTurnDiscountUndo();

            isFirst = false;
        }
        BattleManager.BattleUI.UI_turnNotify.SetUnitTurn();
    }
}
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
        GameManager.UI.ShowScene<UI_TurnNotify>().SetPlayerTurnImage();
        BattleManager.Instance.TurnStart();
    }

    public override void OnStateUpdate()
    {
        BattleManager.Data.BattleUnitOrder();
    }

    public override void OnClickEvent()
    {
        if (isFirst)
        {
            BattleManager.Instance.StartPhase();
        }
        else
        {
            BattleManager.Instance.PreparePhase();
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
        GameManager.UI.ShowScene<UI_TurnNotify>().SetUnitTurnImage();
    }
}
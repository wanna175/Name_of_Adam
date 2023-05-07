public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        if (BattleManager.Phase.Current != BattleManager.Phase.BattleOver)
            BattleManager.Instance.EngagePhase();

        BattleUnit unit = BattleManager.Data.GetNowUnit();
        if (unit != null)
        {
            GameManager.UI.ShowPopup<UI_Info>().Set(unit.DeckUnit, unit.Team, unit.HP.FillAmount(), unit.Fall.GetCurrentFallCount());
        }

    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        GameManager.UI.ClosePopup();
    }
}
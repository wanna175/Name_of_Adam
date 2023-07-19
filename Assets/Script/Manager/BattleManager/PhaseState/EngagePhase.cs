public class EngagePhase : Phase
{
    private UI_Info _engageInfo;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("Stage_Transition/Engage/EngageEnter");
        if (BattleManager.Phase.Current != BattleManager.Phase.BattleOver)
            BattleManager.Instance.EngagePhaseClick();

        BattleUnit unit = BattleManager.Data.GetNowUnit();
        if (unit != null)
        {
            _engageInfo = BattleManager.BattleUI.ShowInfo();
            _engageInfo.SetInfo(unit);
        }
        BattleManager.BattleUI.ChangeButtonName();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        BattleManager.BattleUI.CloseInfo(_engageInfo);
    }
}
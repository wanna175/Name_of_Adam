public class PreparePhase : Phase
{
    private bool isFirst = true;

    public override void OnStateEnter()
    {
        BattleManager.Data.TurnPlus();
        BattleManager.Data.UI_PlayerSkill.Used = false;
        BattleManager.Instance.ChangeButtonName();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (isFirst)
        {
            BattleManager.Instance.StartPhase();
            isFirst = false;
        }
        else
        {
            BattleManager.Instance.PreparePhase();
        }
    }

    public override void OnStateExit()
    {
        BattleManager.Mana.ChangeMana(2);
        BattleManager.Data.BattleUnitOrder();
        
    }
}
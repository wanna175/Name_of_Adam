public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        if(BattleManager.Phase.Current != BattleManager.Phase.BattleOver)
            BattleManager.Instance.EngagePhase();
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        
    }
}
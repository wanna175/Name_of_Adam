public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Instance.EngagePhase();
    }

    public override void OnStateUpdate()
    { 

    }

    public override void OnStateExit()
    {
        
    }
}
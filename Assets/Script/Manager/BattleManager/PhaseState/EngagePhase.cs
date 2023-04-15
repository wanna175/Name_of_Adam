public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {

    }

    public override void OnStateUpdate()
    { 
        BattleManager.Instance.EngagePhase();
    }

    public override void OnStateExit()
    {
        
    }
}
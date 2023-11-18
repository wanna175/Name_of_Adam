public class SetupPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Instance.SetupField();
    }

    public override void OnStateUpdate()
    {
        BattleManager.Phase.ChangePhase(BattleManager.Phase.Spawn);
    }

    public override void OnStateExit()
    {
        
    }
}
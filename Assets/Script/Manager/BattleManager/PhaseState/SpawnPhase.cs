public class SpawnPhase : Phase
{
    public override void OnStateEnter()
    {
        BattleManager.Instance.SpawnInitialUnit();
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {
        
    }
}
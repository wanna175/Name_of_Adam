public class PreparePhase : Phase
{
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Engage);
    }

    public override void OnStateExit()
    {
        _battle.Mana.ChangeMana(2);
        _battle.Data.TurnPlus();
    }
}
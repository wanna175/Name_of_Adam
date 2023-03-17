public class PreparePhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.Data.TurnPlus();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(_controller.Engage);
    }

    public override void OnStateExit()
    {
        _battle.Mana.ChangeMana(2);
    }
}
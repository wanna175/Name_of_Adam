public class StartPhase : Phase
{
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateUpdate()
    {
        if (_battle._clickType >= ClickType.Engage_Nothing)
            _controller.ChangePhase(_controller.Engage);
    }

    public override void OnStateExit()
    {
        
    }
}
public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        _battle.Field.ClearAllColor();
        _battle.Data.BattleUnitOrder();
    }

    public override void OnStateUpdate()
    {
        if (_battle.Data.OrderUnitCount <= 0)
        {
            _controller.ChangePhase(_controller.Prepare);
            _battle.ChangeClickType(ClickType.Prepare_Nothing);// 턴 확인용 임시
            return;
        }
        
        _battle.EngagePhase();
    }

    public override void OnStateExit()
    {
        
    }
}
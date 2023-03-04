public class EngagePhase : Phase
{
    public override void OnStateEnter()
    {
        if (_isFirstExcute == false)
            return;

        _isFirstExcute = false;

        _battle.Field.ClearAllColor();
        _battle.Data.BattleUnitOrder();
    }

    public override void OnStateUpdate()
    {
        _battle.EngagePhase();
    }
}
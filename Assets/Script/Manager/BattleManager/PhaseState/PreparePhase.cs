public class PreparePhase : Phase
{
    private bool isFirst = true;

    public override void OnStateEnter()
    {
        _battle.Data.TurnPlus();
        _battle.Data.UI_PlayerSkill.Used = false;
    }

    public override void OnStateUpdate()
    {
        
    }

    public override void OnClickEvent()
    {
        if (isFirst)
        {
            _battle.StartPhase();
            isFirst = false;
        }
        else
        {
            _battle.PreparePhase();
        }
    }

    public override void OnStateExit()
    {
        _battle.Mana.ChangeMana(2);
        _battle.Data.BattleUnitOrder();
        
    }
}
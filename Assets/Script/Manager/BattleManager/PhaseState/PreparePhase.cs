public class PreparePhase : Phase
{
    public bool isFirst = true;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("Stage_Transition/Prepare/PrepareEnter");
        BattleManager.Mana.ChangeMana(15);
        BattleManager.Data.TurnPlus();
        BattleManager.Data.UI_PlayerSkill.Used = false;
        BattleManager.Instance.ChangeButtonName();
        GameManager.UI.ShowScene<UI_TurnNotify>().SetPlayerTurnImage();
    }

    public override void OnStateUpdate()
    {
        BattleManager.Data.BattleUnitOrder();
    }

    public override void OnClickEvent()
    {
        if (isFirst)
        {
            BattleManager.Instance.StartPhase();
        }
        else
        {
            BattleManager.Instance.PreparePhase();
        }
    }

    public override void OnStateExit()
    {
        isFirst = false;

        GameManager.UI.ShowScene<UI_TurnNotify>().SetUnitTurnImage();
    }
}
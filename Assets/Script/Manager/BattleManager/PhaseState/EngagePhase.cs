using UnityEngine;

public class EngagePhase : Phase
{
    private UI_Info _engageInfo;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("Stage_Transition/Engage/EngageEnter");

        BattleManager.BattleUI.CloseInfo(_engageInfo);

        BattleManager.Instance.BattleOverCheck();

        if (BattleManager.Data.OrderUnitCount <= 0)
        {
            _controller.ChangePhase(BattleManager.Phase.End);
            return;
        }

        BattleManager.BattleUI.ShowTutorial();
    }

    public override void OnStateUpdate()
    {
        _controller.ChangePhase(BattleManager.Phase.Move);
    }

    public override void OnStateExit()
    {
        /*
        BattleUnit unit = BattleManager.Data.GetNowUnit();
        if (unit != null)
        {
            _engageInfo = BattleManager.BattleUI.ShowInfo();
            _engageInfo.SetInfo(unit);
        }
        */
    }
}
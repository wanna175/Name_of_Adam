using UnityEngine;

public class EngagePhase : Phase
{
    private UI_Info _engageInfo;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("Stage_Transition/Engage/EngageEnter");

        BattleManager.BattleUI.CloseInfo(_engageInfo);
        BattleManager.BattleUI.UI_turnNotify.SetBattlePhaseDisplay();

        BattleManager.Data.BattleUnitOrderSorting();

        if (BattleManager.Data.OrderUnitCount <= 0)
        {
            BattleManager.Phase.ChangePhase(BattleManager.Phase.End);
            return;
        }
    }

    public override void OnStateUpdate()
    {
        BattleManager.Phase.ChangePhase(BattleManager.Phase.Move);
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
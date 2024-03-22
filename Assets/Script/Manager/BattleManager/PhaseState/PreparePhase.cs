using UnityEngine;

public class PreparePhase : Phase
{
    private bool _isFirst = true;

    public override void OnStateEnter()
    {
        //GameManager.Sound.Play("Stage_Transition/Prepare/Prepare_Enter_2nd");
        if (!_isFirst)
        { 
            BattleManager.Mana.ChangeMana(30);
        }
        BattleManager.Data.TurnPlus();
        BattleManager.BattleUI.UI_playerSkill.InableSkill(false);
        BattleManager.Mana.ManaInableCheck();
        BattleManager.BattleUI.UI_turnNotify.SetPlayerTurn();
        BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(true);
        BattleManager.BattleUI.UI_controlBar.ControlBarActive();

        if (!GameManager.OutGameData.IsTutorialClear())
        {
            if (TutorialManager.Instance.CheckStep(TutorialStep.UI_PlayerTurn))
            {
                // 첫번째 튜토리얼 설정
                Stat stat = new Stat();
                stat.MaxHP = stat.CurrentHP = -20;
                BattleManager.Data.BattleUnitList[0].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[0].HP.Init(10, 10);
                TutorialManager.Instance.ShowTutorial();

            }
            else if (TutorialManager.Instance.CheckStep(TutorialStep.UI_FallSystem))
            {
                // 두번째 튜토리얼 설정
                Stat stat = new Stat();
                stat.MaxHP = stat.CurrentHP = -15;
                stat.SPD = -50;
                BattleManager.Data.BattleUnitList[1].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[1].HP.Init(5, 5);
                BattleManager.Data.BattleUnitList[0].ChangeFall(1);
                TutorialManager.Instance.ShowTutorial();
            }
            else if (TutorialManager.Instance.CheckStep(TutorialStep.UI_Defeat))
                // 세번째 튜토리얼 설정
                TutorialManager.Instance.ShowTutorial();
            else
                TutorialManager.Instance.ShowNextTutorial();
        }

        BattleManager.Data.BattleUnitOrderReplace();
        BattleManager.Instance.FieldActiveEventCheck(ActiveTiming.TURN_START);
    }

    public override void OnStateUpdate()
    {
        BattleManager.Data.BattleUnitOrderSorting();
    }

    public override void OnClickEvent(Vector2 coord)
    {
        BattleManager.Instance.PreparePhaseClick(coord);
    }

    public override void OnStateExit()
    {
        if (_isFirst) 
        {
            foreach (DeckUnit unit in BattleManager.Data.PlayerDeck)
                unit.FirstTurnDiscountUndo();

            foreach (DeckUnit unit in BattleManager.Data.PlayerHands)
                unit.FirstTurnDiscountUndo();

            BattleManager.BattleUI.RefreshHand();
            _isFirst = false;
        }

        BattleManager.BattleUI.CancelAllSelect();
        BattleManager.BattleUI.UI_turnNotify.SetUnitTurn();
        BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(false);
        BattleManager.PlayerSkillController.SetSkillDone();
        BattleManager.BattleUI.UI_playerSkill.InableSkill(true);
        BattleManager.BattleUI.UI_hands.InableCard(true);
        BattleManager.BattleUI.UI_controlBar.ControlBarInactive();
    }
}
using UnityEngine;

public class PreparePhase : Phase
{
    private bool _isFirst = true;

    public override void OnStateEnter()
    {
        GameManager.Sound.Play("UI/UISFX/UIPlayerTurnSFX");

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

        BattleManager.Field.SetTileHighlightFrame(null, false);

        if (TutorialManager.Instance.IsTutorialOn())
        {
            GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

            if (TutorialManager.Instance.CheckStep(TutorialStep.Start_FirstStage))
            {
                // ù��° Ʃ�丮�� ����
                Stat stat = new();
                stat.MaxHP = stat.CurrentHP = -20;
                BattleManager.Data.BattleUnitList[0].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[0].HP.Init(10, 10);
            }
            else if (TutorialManager.Instance.CheckStep(TutorialStep.Start_SecondStage))
            {
                // �ι�° Ʃ�丮�� ����
                Stat stat = new();
                stat.MaxHP = stat.CurrentHP = -15;
                stat.SPD = -50;
                BattleManager.Data.BattleUnitList[1].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[1].HP.Init(5, 5);
                BattleManager.Data.BattleUnitList[0].ChangeFall(1, null, FallAnimMode.Off);
            }
            else if (TutorialManager.Instance.CheckStep(TutorialStep.Start_ThirdStage))
            {
                Stat stat = new();
                stat.MaxHP = stat.CurrentHP = -15;
                BattleManager.Data.BattleUnitList[0].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[0].HP.Init(5, 5);
            }
            
            TutorialManager.Instance.ShowNextTutorial();
        }

        BattleManager.Data.BattleUnitActionReset();
        BattleManager.Data.BattleUnitOrderReset();
        BattleManager.BattleUI.SetWaitingPlayer(true);
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

        GameManager.Sound.Play("UI/UISFX/UIUnitTurnSFX");

        BattleManager.BattleUI.CancelAllSelect();
        BattleManager.BattleUI.UI_turnNotify.SetUnitTurn();
        BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(false);
        BattleManager.PlayerSkillController.SetSkillDone();
        BattleManager.BattleUI.UI_playerSkill.InableSkill(true);
        BattleManager.BattleUI.UI_hands.InableCard(true);
        BattleManager.BattleUI.UI_controlBar.ControlBarInactive();
        BattleManager.BattleUI.SetWaitingPlayer(false);
    }
}
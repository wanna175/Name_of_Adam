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
        BattleManager.BattleUI.UI_playerSkill.Used = false;
        BattleManager.BattleUI.UI_playerSkill.InableSkill();
        BattleManager.BattleUI.UI_turnNotify.SetPlayerTurn();

        if (!GameManager.OutGameData.isTutorialClear())
        {
            if (TutorialManager.Instance.CheckStep(TutorialStep.UI_PlayerTurn))
            {
                Stat stat = new Stat();
                stat.MaxHP = stat.CurrentHP = -20;
                BattleManager.Data.BattleUnitList[0].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[0].HP.Init(10, 10);
                TutorialManager.Instance.ShowTutorial();

            }
            else if (TutorialManager.Instance.CheckStep(TutorialStep.UI_FallSystem))
            {
                Stat stat = new Stat();
                stat.MaxHP = stat.CurrentHP = -15;
                BattleManager.Data.BattleUnitList[1].DeckUnit.DeckUnitChangedStat += stat;
                BattleManager.Data.BattleUnitList[1].HP.Init(5, 5);
                BattleManager.Data.BattleUnitList[0].ChangeFall(2);
                TutorialManager.Instance.ShowTutorial();
            }
            else
                TutorialManager.Instance.ShowNextTutorial();
        }

        BattleManager.Instance.TurnStart();
    }

    public override void OnStateUpdate()
    {
        BattleManager.Data.BattleUnitOrder();
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
        BattleManager.PlayerSkillController.SetSkillDone();
    }
}
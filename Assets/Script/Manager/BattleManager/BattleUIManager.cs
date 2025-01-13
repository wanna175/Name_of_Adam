using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public UI_ControlBar UI_controlBar;

    public UI_Hands UI_hands;
    public UI_PlayerSkill UI_playerSkill;
    public UI_PlayerHP UI_playerHP;
    public UI_DarkEssence UI_darkEssence;
    public UI_ManaGauge UI_manaGauge;

    public UI_WaitingLine UI_waitingLine;
    public UI_TurnCount UI_turnCount;
    public UI_TurnNotify UI_turnNotify;
    public UI_TurnChangeButton UI_TurnChangeButton;

    public Animator UI_animator;

    private const int _maxHandCount = 3;

    private void Start()
    {
        //버튼 생성
        GameManager.UI.ShowScene<UI_DeckButton>();
        //GameManager.UI.ShowScene<UI_BattleSpeedButton>().SetBattleSpeed(GameManager.OutGameData.GetBattleSpeed());

        //정보들
        UI_waitingLine = GameManager.UI.ShowScene<UI_WaitingLine>();
        //UI_turnCount = GameManager.UI.ShowScene<UI_TurnCount>();
        UI_turnNotify = GameManager.UI.ShowScene<UI_TurnNotify>();
        UI_TurnChangeButton = GameManager.UI.ShowScene<UI_TurnChangeButton>();

        //컨트롤바
        UI_controlBar = GameManager.UI.ShowScene<UI_ControlBar>();
        UI_controlBar.Init();

        UI_playerHP = UI_controlBar.UI_PlayerHP;
        UI_playerSkill = UI_controlBar.UI_PlayerSkill;
        UI_playerSkill.Init(GameManager.Data.GetPlayerSkillList());
        
        UI_hands = UI_controlBar.UI_Hands;
        
        UI_darkEssence = UI_controlBar.UI_DarkEssence;
        UI_darkEssence.Init();

        UI_manaGauge = UI_controlBar.UI_ManaGauge;
        UI_manaGauge.Init();

        UI_animator = UI_controlBar.UI_Aniamtor;

        FillHand();
    }

    public void WaitingLineReset(List<(BattleUnit, int?)> orderList)
    {
        UI_waitingLine.ResetWaitingLine(orderList);
    }

    public void WaitingLineAddOrder((BattleUnit, int?) order)
    {
        UI_waitingLine.AddUnitOrder(order);
    }

    public void WaitingLineRemoveOrder((BattleUnit, int?) order)
    {
        UI_waitingLine.RemoveUnitOrder(order);
    }

    public void WaitingLineChangeCheck(List<(BattleUnit, int?)> orderList)
    {
        UI_waitingLine.UnitOrderChangeCheck(orderList);
    }

    public void WaitingLineRefresh()
    {
        UI_waitingLine.RefreshWaitingUnit();
    }

    public void SetWaitingPlayer(bool on)
    {
        UI_waitingLine.SetWaitingPlayer(on);
    }

    public void AddHandUnit(DeckUnit unit)
    {
        BattleManager.Data.PlayerHands.Add(unit);
        UI_hands.AddUnit(unit);
    }

    public void RemoveHandUnit(DeckUnit unit)
    {
        BattleManager.Data.PlayerHands.Remove(unit);
        UI_hands.RemoveUnit(unit);
        FillHand();
    }

    public void SetFirstTurnNotify(bool isOn)
    {
        UI_controlBar.FirstTurnNotifyAnimator.SetBool("isFadeIn", isOn);//true일 때 꺼지고, false일때 켜짐
    }

    public void FillHand()
    {
        int curID = GameManager.Data.Map.CurrentTileID;

        while (BattleManager.Data.PlayerHands.Count < _maxHandCount)
        {
            DeckUnit unit;
            if (curID == 1 && GameManager.Data.StageAct == 0)
            {
                unit = BattleManager.Data.GetUnitFromDeck();
            }
            else if (curID == 2 && GameManager.Data.StageAct == 0)
            {
                unit = BattleManager.Data.GetTutorialUnitFromDeck();
            }
            else
            {
                unit = BattleManager.Data.GetUnitFromDeck();
                //unit = BattleManager.Data.GetRandomUnitFromDeck();
            }

            if (unit == null)
                return;
            AddHandUnit(unit);
        }
    }

    public void RefreshHand()
    {
        UI_hands.RefreshCard();
    }

    public PlayerSkill GetSelectedPlayerSkill() => UI_playerSkill.GetSelectedCard().GetSkill();

    public void CancelAllSelect()
    {
        BattleManager.Field.ClearAllColor();
        UI_hands.CancelSelect();
        UI_playerSkill.CancelSelect();

        if (BattleManager.PlayerSkillController.IsSkillOn)
            BattleManager.PlayerSkillController.SetSkillDone();
    }

    private List<UI_Info> infoList = new();

    public UI_Info ShowInfo()
    { 
        UI_Info ui_Info = GameManager.UI.ShowScene<UI_Info>();
        ui_Info.GetComponent<Canvas>().sortingOrder = infoList.Count + 1;
        infoList.Add(ui_Info);
        return ui_Info;
    }

    public void CloseInfo(UI_Info close)
    {
        foreach (UI_Info info in infoList)
        {
            info.InfoDestroy();
            infoList.Remove(info);

            break;

            if (info == close)
            {
                info.InfoDestroy();
                infoList.Remove(info);

                break;
            }
        }

        InfoOrdering();
    }

    private void InfoOrdering()
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            infoList[i].GetComponent<Canvas>().sortingOrder = i + 1;
        }
    }
}
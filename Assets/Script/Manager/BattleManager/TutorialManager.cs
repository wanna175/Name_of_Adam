using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// 툴팁에 출력될 문자열
    /// [END]로 끝나는 문자열은 툴팁을 넘길 수 없는(종료된) 상태
    /// 즉, 유저의 특정 행동으로 다음 튜토리얼 진행 가능
    /// </summary>
    private readonly string[] TooltipTexts =
    {
        "<color=#9696FF>마나<color=white>는 유닛을 소환하거나 스킬을 사용할 때 필요합니다.\n플레이어 턴이 시작될 때마다 <color=#FF9696>30<color=white> 회복합니다.",
        "왼쪽 하단에서 현재 보유한 유닛들을 확인할 수 있습니다.\n<color=#9696FF>첫번째 플레이어 턴<color=white>에는 <color=#FF9696>절반의 마나<color=white>를 사용하여 유닛을 소환할 수 있습니다.",
        "오른쪽 하단에서 전투를 보조하는 스킬들을 확인할 수 있습니다.\n전략적으로 사용하여 전투를 주도할 수 있습니다.",
        "덱에서 묘지기 유닛을 선택하세요.[CTRL]",
        "파란색 타일을 클릭하여 유닛을 소환하세요.[CTRL]",
        "이제 턴을 종료해보세요.[CTRL]",
        "유닛 턴에는 필드에 있는 각 유닛들이 속도에 따라 움직입니다.\n우측의 속도표에서 상단에 있는 유닛일수록 먼저 행동합니다.",
        "각 유닛은 한칸 이동 후 적을 공격할 수 있습니다.\n만약 이동이나 공격을 하지 않고 싶다면 턴 종료 버튼을 눌러 턴을 넘길 수도 있습니다.\n묘지기를 앞으로 한칸 이동시켜보세요.[CTRL]",
        "이제 검병을 공격해 처치하세요.[CTRL]",
        "==== 스테이지 1 튜토리얼 종료 ====",


    };

    public const int STEP_BOUNDARY = 1000;

    private static TutorialManager _instance;
    public static TutorialManager Instance
    {
        set
        {
            if (_instance == null)
                _instance = value;
        }
        get
        {
            return _instance;
        }
    }

    [SerializeField]
    private UI_Tutorial UI;

    [SerializeField]
    private TutorialStep _step;

    public TutorialStep Step => _step;

    public bool Tutorial_Trigger_First = true;
    public bool Tutorial_Trigger_Second = true;
    public bool Tutorial_Benediction_Trigger = true;
    public bool Tutorial_Stage_Trigger = true;
    public bool isTutorialactive = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int curID = GameManager.Data.Map.CurrentTileID;

        if (curID == 1 && GameManager.Data.StageAct == 0)
        {
            switch (GameManager.Data.StageAct)
            {
                case 0: _step = TutorialStep.UI_PlayerTurn; break;
                case 1: _step = TutorialStep.UI_FallSystem; break;
            }
        }
    }

    private void Update()
    {
        if (UI.ValidToPassTooltip)
        {
            if (GameManager.InputManager.Click)
            {
                ShowNextTutorial();
            }
        }
    }

    public void ShowNextTutorial()
    {
        SetNextStep();
        ShowTutorial();
    }

    private void SetNextStep()
    {
        TutorialStep[] steps = (TutorialStep[])Enum.GetValues(typeof(TutorialStep));
        int next = Array.IndexOf(steps, _step) + 1;
        _step = (steps.Length == next) ? steps[0] : steps[next];
    }

    private bool IsToolTip(TutorialStep step) 
        => (int)step % STEP_BOUNDARY != 0;

    private Tooltip AnalyzeStep(TutorialStep step)
    {
        Tooltip tooltip = new Tooltip();
        int indexToTooltip = (int)step % STEP_BOUNDARY - 1;

        tooltip.info = TooltipTexts[indexToTooltip].Replace("[CTRL]", "");
        tooltip.indexToTooltip = indexToTooltip;
        tooltip.isCtrl = TooltipTexts[indexToTooltip].Contains("[CTRL]");
        tooltip.isEnd = false;

        if (CheckStep(TutorialStep.Tutorial_End))
            tooltip.isEnd = true;

        return tooltip;
    }

    public bool CheckStep(TutorialStep step) => this.Step == step;

    public void ShowTutorial()
    {
        if (IsToolTip(_step))
        {
            Tooltip tooltip;
            tooltip = AnalyzeStep(_step);

            if (tooltip.isEnd)
            {
                UI.CloseToolTip();
                UI.SetUIMask(-1);
                UI.SetValidToPassToolTip(false);
                SetActiveAllTiles(true);
            }
            else
            {
                UI.ShowTooltip(tooltip.info, tooltip.indexToTooltip);
                UI.SetUIMask(tooltip.indexToTooltip);
                UI.SetValidToPassToolTip(!tooltip.isCtrl);
                SetTutorialField();
            }
        }
        else
        {
            switch (_step)
            {
                case TutorialStep.UI_PlayerTurn:
                    UI.TutorialActive(0);
                    break;
                case TutorialStep.UI_FallSystem:
                    UI.TutorialActive(1);
                    break;
            }
        }

        //if (curID == 1 && GameManager.Data.StageAct == 0)
        //{
        //    if (Tutorial_Trigger_First == true)
        //    {
        //        if (phaseController.CurrentPhaseCheck(phaseController.Prepare))
        //        {
        //            UI.TutorialActive(0);
        //        }
        //        else if (phaseController.CurrentPhaseCheck(phaseController.Engage))
        //        {
        //            UI.TutorialActive(1);
        //            Tutorial_Trigger_First = false;
        //        }
        //    }
        //}
        //else if (curID == 2 && GameManager.Data.StageAct == 0)
        //{
        //    if (Tutorial_Trigger_Second == true)
        //    {
        //        if (phaseController.Current == phaseController.Prepare)
        //        {
        //            UI.TutorialActive(8);
        //        }
        //        else if (phaseController.Current == phaseController.Engage)
        //        {
        //            UI.TutorialActive(12);
        //            Tutorial_Trigger_Second = false;
        //        }
        //    }
        //}
    }

    private void SetTutorialField()
    {
        SetActiveAllTiles(false);

        switch (_step)
        {
            case TutorialStep.Tooltip_UnitSpawnSelect:
                BattleManager.Field.TileDict[new Vector2(1, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitMove:
                BattleManager.Field.TileDict[new Vector2(2, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
        }
    }

    private void SetActiveAllTiles(bool isActive)
    {
        foreach (Tile tile in BattleManager.Field.TileDict.Values)
            tile.SetActiveCollider(isActive);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// 툴팁에 출력될 문자열
    /// [CTRL]로 끝나는 문자열은 유저가 직접 액션을 하는 단계를 의미
    /// 즉, 유저의 특정 행동으로 다음 튜토리얼 진행 가능
    /// </summary>
    private readonly string[] TooltipTexts =
    {
        // 튜토리얼 1 시작
        "<color=#9696FF>마나<color=white>는 유닛을 소환하거나 스킬을 사용할 때 필요합니다.\n플레이어 턴이 시작될 때마다 <color=#FF9696>30<color=white> 회복합니다.",
        "왼쪽 하단에서 현재 보유한 유닛들을 확인할 수 있습니다.\n<color=#9696FF>첫번째 플레이어 턴<color=white>에는 <color=#FF9696>절반의 마나<color=white>를 사용하여 유닛을 소환할 수 있습니다.",
        "오른쪽 하단에서 전투를 보조하는 스킬들을 확인할 수 있습니다.\n전략적으로 사용하여 전투를 주도할 수 있습니다.",
        "덱에서 묘지기 유닛을 선택하세요.[CTRL]",
        "파란색 타일을 클릭하여 유닛을 소환하세요.[CTRL]",
        "이제 턴을 종료해보세요.[CTRL]",
        "유닛 턴에는 필드에 있는 각 유닛들이 속도에 따라 움직입니다.\n우측의 속도표에서 상단에 있는 유닛일수록 먼저 행동합니다.",
        "각 유닛은 한칸 이동 후 적을 공격할 수 있습니다.\n만약 이동이나 공격을 하지 않고 싶다면 턴 종료 버튼을 눌러 턴을 넘길 수도 있습니다.\n묘지기를 앞으로 한칸 이동시켜보세요.[CTRL]",
        "이제 검병을 공격해 처치하세요.[CTRL]",

        // 튜토리얼 2 시작
        "체력바 옆에 있는 보석은 신앙입니다.\n신앙이 전부 깎일 시 적은 타락하여 아군이 됩니다. 반대의 경우 아군이 적이 됩니다.",
        "특정 유닛을 사용하거나 스킬을 사용할 때 필요한 검은 정수입니다.\n적을 처치할때마다 하나씩 얻을 수 있습니다.",
        "흑기사는 적을 타락하는데 유용한 낙인을 지닌 강한 유닛이며\n 마나뿐만 아니라 검은 정수까지 소모합니다. 흑기사를 선택하세요.[CTRL]",
        "이제 타일을 선택하여 흑기사를 소환하세요.[CTRL]",
        "공격 시 적의 신앙을 떨어뜨리는 악성 버프입니다.\n흑기사는 이 악성 버프를 2회 얻는 낙인을 가지고 있습니다. 잘 활용하여 적을 타락시켜보세요.",
        "턴 종료를 눌러 유닛 턴으로 넘어가세요.[CTRL]",
        "만약 유닛을 이동할 필요가 없는 경우엔 턴 종료를 눌러 턴을 넘길 수 있어요.[CTRL]",
        "검병이 놓인 타일을 클릭하여 공격하세요.[CTRL]",
        "이번 턴에서는 플레이어 스킬을 사용해보겠습니다.\n속삭임 스킬을 선택하세요.[CTRL]",
        "검병에게 사용하여 타락시켜보세요.[CTRL]",
        "적을 타락시킬 경우 낙인을 부여하여 아군으로 만들 수 있습니다.\n검병에게 부여할 낙인을 선택해보세요.[CTRL]",
        "이제 검병은 당신의 유닛이 되었습니다!\n이제 턴 종료를 누르세요.[CTRL]",
        "아군이 이미 있는 위치를 클릭할 시 두 유닛은 서로 위치를 변경합니다.\n검병을 클릭해 흑기사와 위치를 변경하세요.[CTRL]",
        "수녀를 공격하세요.[CTRL]",
        "흑기사를 클릭해 겸벙과 위치를 변경하세요.[CTRL]",
        "수녀를 처치하세요.[CTRL]",
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

    private TooltipData currentTooltip;

    private bool isEnable;

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

        switch (curID)
        {
            case 1: _step = TutorialStep.UI_PlayerTurn; break;
            case 2: _step = TutorialStep.UI_FallSystem; break;
        }
    }

    private void Update()
    {
        if (!isEnable)
            return;

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

    public bool IsEnable()
        => !GameManager.OutGameData.isTutorialClear() && isEnable;

    private void SetNextStep()
    {
        TutorialStep[] steps = (TutorialStep[])Enum.GetValues(typeof(TutorialStep));
        int next = Array.IndexOf(steps, _step) + 1;
        _step = (steps.Length == next) ? steps[0] : steps[next];
    }

    private bool IsToolTip(TutorialStep step)
        => (int)step % STEP_BOUNDARY != 0;

    private TooltipData AnalyzeStep(TutorialStep step)
    {
        TooltipData tooltip = new TooltipData();
        int indexToTooltip = (int)step % STEP_BOUNDARY - 1;

        tooltip.Step = step;
        tooltip.Info = TooltipTexts[indexToTooltip].Replace("[CTRL]", "");
        tooltip.IndexToTooltip = indexToTooltip;
        tooltip.IsCtrl = TooltipTexts[indexToTooltip].Contains("[CTRL]");
        tooltip.IsEnd = false;

        if (CheckStep(TutorialStep.Tutorial_End_1) || CheckStep(TutorialStep.Tutorial_End_2))
            tooltip.IsEnd = true;

        return tooltip;
    }

    public bool CheckStep(TutorialStep step) => this.Step == step;

    public void ShowTutorial()
    {
        if (IsToolTip(_step))
        {
            // Tooltip 모드
            currentTooltip = AnalyzeStep(_step);
            if (currentTooltip.IsEnd)
                DisableToolTip();
            else
                EnableToolTip(currentTooltip);
        }
        else
        {
            // UI 모드
            isEnable = true;
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

    public void DisableToolTip()
    {
        UI.CloseToolTip();
        UI.SetUIMask(-1);
        UI.SetValidToPassToolTip(false);
        SetActiveAllTiles(true);
        isEnable = false;
    }

    public void EnableToolTip(TooltipData data)
    {
        UI.ShowTooltip(data.Info, data.IndexToTooltip);
        UI.SetUIMask(data.IndexToTooltip);
        UI.SetValidToPassToolTip(!data.IsCtrl);
        SetTutorialField(data.Step);
        isEnable = true;
    }

    private void SetTutorialField(TutorialStep step)
    {
        Debug.Log(Step);
        SetActiveAllTiles(false);

        switch (step)
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
            case TutorialStep.Tooltip_BlackKnightSpawn:
                BattleManager.Field.TileDict[new Vector2(2, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack_2:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_PlayerSkillUse:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitSwap:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack_3:
                BattleManager.Field.TileDict[new Vector2(4, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitSwap_2:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack_4:
                BattleManager.Field.TileDict[new Vector2(4, 1)].SetActiveCollider(true);
                break;
        }
    }

    private void SetActiveAllTiles(bool isActive)
    {
        foreach (Tile tile in BattleManager.Field.TileDict.Values)
            tile.SetActiveCollider(isActive);
    }
}

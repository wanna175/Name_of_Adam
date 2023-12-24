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
        "덱에서 묘지기 유닛을 선택하세요.[END]",
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
            _step = TutorialStep.UI_PlayerTurn;
        }
    }

    private void Update()
    {
        if (UI.isWorkableTooltip)
        {
            if (GameManager.InputManager.Click)
            {
                ShowTutorial();
            }
        }
    }

    public void SetNextStep()
    {
        TutorialStep[] steps = (TutorialStep[])System.Enum.GetValues(typeof(TutorialStep));
        int next = System.Array.IndexOf(steps, _step) + 1;
        _step = (steps.Length == next) ? steps[0] : steps[next];
    }

    private bool IsToolTip(TutorialStep step) => (int)step < STEP_BOUNDARY;

    private string GetInfoText(string tooltipText, out bool isEnd)
    {
        isEnd = tooltipText.Contains("[END]");
        return tooltipText.Replace("[END]", "");
    }

    public void ShowTutorial()
    {
        int curID = GameManager.Data.Map.CurrentTileID;

        if (curID == 1 && GameManager.Data.StageAct == 0)
        {
            if (IsToolTip(_step))
            {
                bool isEnd;
                string infoText = GetInfoText(TooltipTexts[(int)_step], out isEnd);
                UI.ShowTooltip(infoText);
                UI.SetWorkableToolTip(!isEnd);
                SetNextStep();
            }
            else
            {
                switch (_step)
                {
                    case TutorialStep.UI_PlayerTurn:
                        UI.TutorialActive(0);
                        break;
                }
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
}

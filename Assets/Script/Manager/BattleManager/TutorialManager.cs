using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// 툴팁에 출력될 문자열
    /// [CTRL]로 끝나는 문자열은 유저가 직접 액션을 하는 단계를 의미
    /// 즉, 유저의 특정 행동으로 다음 튜토리얼 진행 가능
    /// </summary>
    private readonly string[][] TooltipTexts =
    {
        // 영문
        new string[] {
        // 튜토리얼 1 시작
        "During the <color=#FF9696>Preparation Phase<color=white>, you can summon units or use skills",
        "<color=#FF9696>Mana<color=white> is required to summon units or use skills\nMana regenerates by <color=#FF9696>30<color=white> at the start of each Preparation Phase",
        "These are the units you can currently summon\nDuring the <color=#FF9696>first Preparation Phase<color=white>, you can summon units using only <color=#FF9696>half of the required mana<color=white>",
        "These are the skills that aid you in combat",
        "Summon a Gravekeeper[CTRL]",
        "Summon a Gravekeeper[CTRL]",
        "When the Preparation Phase ends, the <color=#FF9696>Battle Phase<color=white> comes[CTRL]",
        "During the <color=#FF9696>Battle Phase<color=white>, units on the field move according to their speed\nUnits with the higher speed act first\nYou can check the order of the units in the <color=#FF9696>speed bar<color=white> above",
        "Each unit can move on its turn\nMove the Gravekeeper one step forward[CTRL]",
        "After the movement turn comes the attack turn\nAttack the Swordsman[CTRL]",

        // 튜토리얼 2 시작
        "<color=#FF9696>Dark Essence<color=white> is required to summon powerful units or use skills\nDark Essence can be obtained by defeating enemies\nYou can earn one Dark Essence per enemy",
        "The Dark Knight is a powerful unit that requires not only mana but also Dark Essence,\n and it has the <color=#FF9696>‘Malevolence’</color> Stigmata which is useful when corrupting enemies\nSelect the Dark Knight[CTRL]",
        "The Dark Knight is a powerful unit that requires not only mana but also Dark Essence,\n and it has the <color=#FF9696>‘Malevolence’</color> Stigmata which is useful when corrupting enemies\nSelect the Dark Knight[CTRL]",
        "The <color=#FF9696>malevolence buff<color=white> reduces an enemy's <color=#FF9696>faith<color=white> when attacking\nThe Dark Knight has the stigmata that provides the malevolence <color=#FF9696>buff twice<color=white>\nEffectively utilize these instructions to corrupt enemies",
        "Click the <color=#FF9696>Phase End button</color> to move to the Battle Phase[CTRL]",
        "You can press the <color=#FF9696>Skip Move button <color=white>to skip the movement turn.[CTRL]",
        "Attack the swordsman to reduce <color=#FF9696>faith<color=white>[CTRL]",
        "Now, it is the Preparation Phase\nUse the skill <color=#FF9696>'Whisper'</color> to reduce the enemy's faith and corrupt them[CTRL]",
        "Now, it is the Preparation Phase\nUse the skill <color=#FF9696>'Whisper'</color> to reduce the enemy's faith and corrupt them[CTRL]",
        "When corrupting an enemy, it becomes your ally, and you can choose a <color=#FF9696>stigmata</color> to bestow upon the unit\nSelect a stigmata to bestow upon the swordsman[CTRL]",
        "The Swordsman is now your unit\nPress the end phase button to start the battle[CTRL]",
        "When moving to a position already occupied by an ally, the two units swap places\nMove the Dark Knight to switch positions.[CTRL]",
        "The Nun has an <color=#FF9696>'Invincibility'</color> stigmata that nullifies an attack once\nAttack the nun to remove the Invincibility buff[CTRL]",
        "Move the swordsman again[CTRL]",
        "Finish the Nun, now that her invincibility buff has been removed[CTRL]",
        "",
        },

        // 한국
        new string[] {
        // 튜토리얼 1 시작
        "<color=#FF9696>준비 단계<color=white>에는 유닛을 소환하거나 스킬을 쓸 수 있습니다",
        "<color=#FF9696>마나<color=white>는 유닛들 소환하거나 스킬을 사용할 때 필요합니다\n준비 단계가 될 때마다 <color=#FF9696>30<color=white>씩 회복됩니다",
        "현재 소환할 수 있는 유닛들입니다\n<color=#FF9696>첫 번째 준비 단계<color=white>에는 <color=#FF9696>절반의 마나<color=white>를 사용하여 유닛을 소환할 수 있습니다",
        "전투를 보조하는 스킬들입니다",
        "묘지기를 소환하세요[CTRL]",
        "묘지기를 소환하세요[CTRL]",
        "준비 단계를 종료하면 <color=#FF9696>전투 단계<color=white>로 넘어갑니다[CTRL]",
        "<color=#FF9696>전투 단계<color=white>에는 각 유닛들이 순서대로 움직입니다\n속도가 높은 유닛일수록 먼저 행동합니다\n순서는 상단의 <color=#FF9696>속도표<color=white>에서 확인할 수 있습니다",
        "각 유닛은 본인 턴에 이동할 수 있습니다\n묘지기를 한 칸 앞으로 이동시키세요[CTRL]",
        "이동 턴 후에는 공격 턴입니다\n검병을 공격하세요[CTRL]",

        // 튜토리얼 2 시작
        "<color=#FF9696>검은 정수<color=white>는 강력한 유닛을 소환하거나 스킬을 사용할 때 필요합니다\n적을 처치하여 하나씩 얻을 수 있습니다.",
        "흑기사는 마나 뿐만 아니라 검은 정수를 소모하는 강력한 유닛으로,\n 적을 타락시키기에 유용한 성흔 <color=#FF9696>'악성'<color=white>을 가졌습니다\n흑기사를 선택하세요.[CTRL]",
        "흑기사는 마나 뿐만 아니라 검은 정수를 소모하는 강력한 유닛으로,\n 적을 타락시키기에 유용한 성흔 <color=#FF9696>'악성'<color=white>을 가졌습니다\n흑기사를 선택하세요.[CTRL]",
        "공격 시 적의 <color=#FF9696>신앙<color=white>을 떨어뜨리는 <color=#FF9696>악성 버프<color=white>입니다\n흑기사는 이 악성 버프를 <color=#FF9696>2회<color=white> 얻는 성흔을 가지고 있습니다\n잘 활용하여 적을 타락시켜보세요",
        "<color=#FF9696>단계 종료 버튼<color=white>을 눌러 전투 단계로 넘어가세요[CTRL]",
        "<color=#FF9696>이동 스킵 버튼<color=white>을 눌러 이동 턴을 넘길 수 있습니다[CTRL]",
        "검병을 공격하여 <color=#FF9696>신앙<color=white>을 떨어뜨리세요[CTRL]",
        "이제 준비 단계가 되었습니다\n신앙을 떨어뜨리는 스킬 <color=#FF9696>'속삭임'<color=white>을 사용하여 적을 타락시키세요[CTRL]",
        "이제 준비 단계가 되었습니다\n신앙을 떨어뜨리는 스킬 <color=#FF9696>'속삭임'<color=white>을 사용하여 적을 타락시키세요[CTRL]",
        "적을 타락시킬 경우 해당 적은 아군이 되며, <color=#FF9696>성흔</color>을 부여할 수 있습니다\n검병에게 부여할 성흔을 선택하세요[CTRL]",
        "이제 검병은 당신의 유닛이 되었습니다\n단계 종료를 눌러, 전투를 시작하세요[CTRL]",
        "이미 아군이 있는 위치로 이동할 시, 두 유닛은 서로 위치를 바꿉니다\n흑기사를 이동시켜 위치를 바꾸세요[CTRL]",
        "수녀는 공격을 1회 막아주는 <color=#FF9696>'무적'</color> 성흔을 가지고 있습니다\n수녀를 공격하여 무적 버프를 없애세요[CTRL]",
        "검병을 다시 이동시키세요[CTRL]",
        "무적 버프가 사라진 수녀를 마무리하세요[CTRL]",
        "",
        },
    };

    private const float RECLICK_TIME = 0.5f;

    private static TutorialManager m_instance;
    public static TutorialManager Instance
    {
        private set
        {
            if (m_instance == null)
                m_instance = value;
        }
        get => m_instance;
    }

    [SerializeField] private UI_Tutorial UI;
    
    private TooltipData _currentTooltip;
    private TutorialStep _step;
    
    private bool _isEnableUpdate;
    private bool _isCanClick;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isCanClick = true;
        
        int curID = GameManager.Data.Map.CurrentTileID;
        switch (curID)
        {
            case 1: _step = TutorialStep.Start_FirstStage; break;
            case 2: _step = TutorialStep.Start_SecondStage; break;
            case 3: _step = TutorialStep.Start_ThirdStage; break;
        }
    }

    private void Update()
    {
        if (!_isEnableUpdate)
            return;

        if (UI.ValidToPassTooltip)
        {
            if (_isCanClick && !GameManager.UI.IsOnESCOption && GameManager.InputManager.Click)
            {
                StartCoroutine(ClickCoolTime());
                ShowNextTutorial();
            }
        }
    }
    
    public bool CheckStep(TutorialStep step) => _step == step;
    public bool IsTutorialOn() => !GameManager.OutGameData.Data.TutorialClear;
    public bool IsEnableUpdate() => IsTutorialOn() && _isEnableUpdate;
    public void SetNextStep() => _step++;

    public void ShowNextTutorial()
    {
        if (CheckStep(TutorialStep.Popup_Last))
            return; // 마지막 UI 튜토리얼 관련 Step은 조건부 동작이기 때문에 예외 처리

        _step++;
        ShowTutorial();
    }

    public void ShowPreviousTutorial()
    {
        _step--;
        ShowTutorial();
    }
    
    public void ShowTutorial()
    {
        TutorialType type = GetTutorialType(_step);
        Debug.Log($"<color=red>[Tutorial] Show Tutorial: {_step} ({type})</color>");

        switch (type)
        {
            case TutorialType.Tooltip:  // Tooltip 모드
                
                GameManager.Sound.Play("UI/UISFX/UIUnimportantButtonSFX");
                
                _currentTooltip = GetTooltipData(_step);
                if (_currentTooltip.IsEnd)
                    DisableToolTip();
                else
                    EnableToolTip(_currentTooltip);
                
                break;
            
            case TutorialType.Popup:    // Popup 모드
                
                int indexToUI = GetPopupIndex(_step);
                UI.TutorialActive(indexToUI);
                _isEnableUpdate = true;
                
                break;
        }
        
        SetTutorialField(_step);
    }

    public void DisableToolTip()
    {
        UI.CloseToolTip();
        UI.SetUIMask(-1);
        UI.SetValidToPassToolTip(false);
        _isEnableUpdate = false;
    }

    public void EnableToolTip(TooltipData data)
    {
        UI.ShowTooltip(data.Info, data.IndexToTooltip);
        UI.SetUIMask(data.IndexToTooltip);
        UI.SetValidToPassToolTip(!data.IsCtrl);
        _isEnableUpdate = true;
    }

    private TooltipData GetTooltipData(TutorialStep step)
    {
        TooltipData tooltip = new();

        int tooltipIndex = GetTooltipIndex(step);

        tooltip.IndexToTooltip = tooltipIndex;
        tooltip.Info = TooltipTexts[GameManager.OutGameData.Data.Language][tooltipIndex].Replace("[CTRL]", "");
        tooltip.IsCtrl = TooltipTexts[GameManager.OutGameData.Data.Language][tooltipIndex].Contains("[CTRL]");
        tooltip.IsEnd = step.ToString().Contains("End_");
        
        return tooltip;
    }
    
    private int GetTooltipIndex(TutorialStep step)
    {
        int index = 0;
        foreach (var enumElement in Enum.GetValues(typeof(TutorialStep)))
        {
            string enumStr = enumElement.ToString();
            
            if (enumStr.Equals(step.ToString()))
                return index;
            if (enumStr.Contains("Tooltip_"))
                index++;
        }

        // 툴입 탐색 실패
        return -1;
    }
    
    private int GetPopupIndex(TutorialStep step)
    {
        int index = 0;
        foreach (var enumElement in Enum.GetValues(typeof(TutorialStep)))
        {
            string enumStr = enumElement.ToString();
            
            if (enumStr.Equals(step.ToString()))
                return index;
            if (enumStr.Contains("Popup_"))
                index++;
        }

        // 팝업 탐색 실패
        return -1;
    }
    
    private TutorialType GetTutorialType(TutorialStep step)
    {
        string stepStr = step.ToString();
        
        if (stepStr.Contains("Start_"))
            return TutorialType.Start;
        if (stepStr.Contains("Popup_"))
            return TutorialType.Popup;
        if (stepStr.Contains("Tooltip_"))
            return TutorialType.Tooltip;
        if (stepStr.Contains("End_"))
            return TutorialType.End;
        
        return TutorialType.None;
    }

    private void SetTutorialField(TutorialStep step)
    {
        BattleManager.Field.SetActiveAllTiles(false);

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
            case TutorialStep.Tooltip_UnitAttack2:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_PlayerSkillUse:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitSwap:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack3:
                BattleManager.Field.TileDict[new Vector2(4, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitSwap2:
                BattleManager.Field.TileDict[new Vector2(3, 1)].SetActiveCollider(true);
                break;
            case TutorialStep.Tooltip_UnitAttack4:
                BattleManager.Field.TileDict[new Vector2(4, 1)].SetActiveCollider(true);
                break;
            
            case TutorialStep.Popup_Defeat:
            case TutorialStep.Popup_Last:
                SetActiveAllTiles(true);
                break;
        }
    }

    private IEnumerator ClickCoolTime()
    {
        _isCanClick = false;
        yield return new WaitForSeconds(RECLICK_TIME);
        _isCanClick = true;
    }
}

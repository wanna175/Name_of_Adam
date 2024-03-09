using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// ������ ��µ� ���ڿ�
    /// [CTRL]�� ������ ���ڿ��� ������ ���� �׼��� �ϴ� �ܰ踦 �ǹ�
    /// ��, ������ Ư�� �ൿ���� ���� Ʃ�丮�� ���� ����
    /// </summary>
    private readonly string[][] TooltipTexts =
    {
        // ����
        new string[] {
        // Ʃ�丮�� 1 ����
        "During the <color=#FF9696>player turn<color=white>, you can summon units or use skills.",
        "<color=#FF9696>Mana<color=white> is required for summoning units or using skills.\nMana recovers by <color=#FF9696>30<color=white> each player turn",
        "These are the currently summonable units.\n<color=#FF9696>On the first player turn<color=white>, you can summon units using only <color=#FF9696>half of<color=white> the required mana.",
        "These are the skills that aid you in combat.",
        "Summon a Gravekeeper.[CTRL]",
        "Summon a Gravekeeper.[CTRL]",
        "When the player turn ends, the <color=#FF9696>unit turn<color=white> comes.[CTRL]",
        "During the <color=#FF9696>unit turn<color=white>, units on the field move according to their speed.\nUnits at the top of the <color=#FF9696>speed bar<color=white> on the right act first.",
        "Each unit can move one step and then attack the enemies.\nMove the Gravekeeper one step forward.[CTRL]",
        "Attack the Swordsman.[CTRL]",

        // Ʃ�丮�� 2 ����
        "This is the <color=#FF9696>dark essence<color=white> needed for using specific units or skills.\nDark essence is obtained by defeating enemies",
        "The <color=#FF9696>dark knight<color=white> is a powerful unit that consumes both mana and <color=#FF9696>Dark Essence.<color=white>\nSummon the Dark Knight.[CTRL]",
        "The <color=#FF9696>dark knight<color=white> is a powerful unit that consumes both mana and <color=#FF9696>Dark Essence.<color=white>\nSummon the Dark Knight.[CTRL]",
        "The <color=#FF9696>malevolence buff<color=white> reduces an enemy's <color=#FF9696>faith<color=white> when attacking.\nThe Dark Knight has the stigmata that provides the malevolence <color=#FF9696>buff twice.<color=white>\nEffectively utilize these instructions to corrupt enemies.",
        "Click the <color=#FF9696>'Turn End' <color=white>button to move to the unit turn.[CTRL]",
        "Press the <color=#FF9696>Turn End button<color=white> to skip to the next turn when moving is unnecessary.[CTRL]",
        "Attack the swordsman to reduce <color=#FF9696>faith.<color=white>[CTRL]",
        "Use the skill <color=#FF9696>Whisper<color=white> to reduce the enemy's faith and  corrupt them.[CTRL]",
        "Use the skill <color=#FF9696>Whisper<color=white> to reduce the enemy's faith and  corrupt them.[CTRL]",
        "When corrupting an enemy, you can choose a <color=#FF9696>stigmata<color=white> to apply and convert them into an ally.\nSelect a stigmata to bestow upon the swordsman.[CTRL]",
        "The swordsman has become your unit. Now, click 'Turn End'.[CTRL]",
        "When a unit moves to a position where an ally already exists, the two units change places.\nMove the dark knight.[CTRL]",
        "Attack the Nun to remove her invincibility buff.[CTRL]",
        "Move the Swordsman.[CTRL]",
        "Finish off the Nun, now that the invincibility buff has disappeared.[CTRL]",
        "",
        },

        // �ѱ�
        new string[] {
        // Ʃ�丮�� 1 ����
        "<color=#FF9696>�÷��̾� ��<color=white>���� ������ ��ȯ�ϰų� ��ų�� �� �� �ֽ��ϴ�.",
        "������ ��ȯ�ϰų� ��ų�� ����Ҷ� �ʿ��� <color=#FF9696>����<color=white>�Դϴ�.\n�÷��̾� ���� �� ������ <color=#FF9696>30<color=white>�� ȸ���մϴ�.",
        "���� ��ȯ�� �� �ִ� ���ֵ��Դϴ�.\n<color=#FF9696>ù��° �÷��̾� ��<color=white>���� <color=#FF9696>������ ����<color=white>�� ����Ͽ� ������ ��ȯ�� �� �ֽ��ϴ�.",
        "������ �����ϴ� ��ų���Դϴ�.",
        "�����⸦ ��ȯ�غ�����.[CTRL]",
        "�����⸦ ��ȯ�غ�����.[CTRL]",
        "���� �����ϸ� <color=#FF9696>���� ��<color=white>���� �Ѿ�ϴ�.[CTRL]",
        "<color=#FF9696>���� ��<color=white>���� �ʵ忡 �ִ� �� ���ֵ��� �ӵ��� ���� �����Դϴ�.\n������ <color=#FF9696>�ӵ�ǥ<color=white>���� ��ܿ� �ִ� �����ϼ��� ���� �ൿ�մϴ�.",
        "�� ������ ��ĭ �̵� �� ���� ������ �� �ֽ��ϴ�.\n�����⸦ ������ ��ĭ �̵����Ѻ�����.[CTRL]",
        "�˺��� �����غ�����.[CTRL]",

        // Ʃ�丮�� 2 ����
        "Ư�� ������ ����ϰų� ��ų�� ����� �� �ʿ��� <color=#FF9696>���� ����<color=white>�Դϴ�.\n���� óġ�� ������ �ϳ��� ���� �� �ֽ��ϴ�.",
        "����� ���� <color=#FF9696>Ÿ��<color=white>�ϴµ� ������ ������ ���� ���� �����̸� �����Ӹ� �ƴ϶� <color=#FF9696>���� ����<color=white>���� �Ҹ��մϴ�.\n���縦 �����ϼ���.[CTRL]",
        "����� ���� <color=#FF9696>Ÿ��<color=white>�ϴµ� ������ ������ ���� ���� �����̸� �����Ӹ� �ƴ϶� <color=#FF9696>���� ����<color=white>���� �Ҹ��մϴ�.\n���縦 �����ϼ���.[CTRL]",
        "���� �� ���� <color=#FF9696>�ž�<color=white>�� ����߸��� <color=#FF9696>�Ǽ� ����<color=white>�Դϴ�.\n����� �� �Ǽ� ������ <color=#FF9696>2ȸ<color=white> ��� ������ ������ �ֽ��ϴ�.\n�� Ȱ���Ͽ� ���� Ÿ�����Ѻ�����.",
        "<color=#FF9696>�� ���� ��ư<color=white>�� ���� ���� ������ �Ѿ����.[CTRL]",
        "�̵��� �ʿ䰡 ���� ��� <color=#FF9696>�� ���� ��ư<color=white>�� ���� ���� �ѱ� �� �־��.[CTRL]",
        "�˺��� �����Ͽ� <color=#FF9696>�ž�<color=white>�� ����߸�����.[CTRL]",
        "<color=#FF9696>�ž�<color=white>�� ����߸��� ��ų <color=#FF9696>�ӻ���<color=white>�� ����Ͽ� ���� Ÿ������ ������.[CTRL]",
        "<color=#FF9696>�ž�<color=white>�� ����߸��� ��ų <color=#FF9696>�ӻ���<color=white>�� ����Ͽ� ���� Ÿ������ ������.[CTRL]",
        "���� Ÿ����ų ��� �ش� ���� �Ʊ����� ����� <color=#FF9696>����<color=white>�� �ο��� �� �ֽ��ϴ�.\n�˺����� �ο��� ������ �����ϼ���.[CTRL]",
        "���� �˺��� ����� ������ �Ǿ����ϴ�.\n���� �� ���Ḧ ��������.[CTRL]",
        "�Ʊ��� �̹� �ִ� ��ġ�� �̵��� �� �� ������ ���� ��ġ�� �ٲߴϴ�.\n���縦 �̵���Ű����.[CTRL]",
        "���ฦ �����Ͽ� ���� ������ ���ֺ�����.[CTRL]",
        "�˺��� �̵���Ű����.[CTRL]",
        "���� ������ ����� ���ฦ �������ϼ���.[CTRL]",
        "",
        },
    };

    public const int STEP_BOUNDARY = 100;

    private const float RECLICK_TIME = 0.5f;

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

    public bool IsTutorialactive;
    private bool isEnable;
    private bool isCanClick;

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
            case 3: _step = TutorialStep.UI_Defeat; break;
        }

        isCanClick = true;
    }

    private void Update()
    {
        if (!isEnable)
            return;

        if (UI.ValidToPassTooltip)
        {
            if (isCanClick && !GameManager.UI.IsOnESCOption && GameManager.InputManager.Click)
            {
                StartCoroutine(ClickCoolTime());
                ShowNextTutorial();
            }
        }
    }

    public void ShowNextTutorial()
    {
        if (CheckStep(TutorialStep.UI_Devine) || CheckStep(TutorialStep.UI_Last))
            return; // ������ UI Ʃ�丮�� ���� Step�� ���Ǻ� �����̱� ������ ���� ó��

        SetNextStep();
        ShowTutorial();
    }

    public void ShowPreviousTutorial()
    {
        SetPreviousStep();
        ShowTutorial();
    }

    public bool IsEnable()
        => !GameManager.OutGameData.IsTutorialClear() && isEnable;

    public void SetNextStep()
    {
        TutorialStep[] steps = (TutorialStep[])Enum.GetValues(typeof(TutorialStep));
        int next = Array.IndexOf(steps, _step) + 1;
        _step = (steps.Length == next) ? steps[0] : steps[next];
    }

    private void SetPreviousStep()
    {
        TutorialStep[] steps = (TutorialStep[])Enum.GetValues(typeof(TutorialStep));
        int next = Array.IndexOf(steps, _step) - 1;
        _step = (steps.Length == -1) ? steps[steps.Length - 1] : steps[next];
    }

    private bool IsToolTip(TutorialStep step)
        => (int)step % STEP_BOUNDARY != 0;

    private TooltipData AnalyzeTooltip(TutorialStep step)
    {
        TooltipData tooltip = new TooltipData();
        int indexToTooltip = (int)step % STEP_BOUNDARY - 1;

        tooltip.Step = step;
        tooltip.Info = TooltipTexts[GameManager.OutGameData.GetLanguage()][indexToTooltip].Replace("[CTRL]", "");
        tooltip.IndexToTooltip = indexToTooltip;
        tooltip.IsCtrl = TooltipTexts[GameManager.OutGameData.GetLanguage()][indexToTooltip].Contains("[CTRL]");
        tooltip.IsEnd = false;

        if (CheckStep(TutorialStep.Tutorial_End_1) || 
            CheckStep(TutorialStep.Tutorial_End_2) || 
            CheckStep(TutorialStep.Tutorial_End_3))
            tooltip.IsEnd = true;

        return tooltip;
    }

    private int AnalyzeUI(TutorialStep step) => (int)step / STEP_BOUNDARY - 1;

    public bool CheckStep(TutorialStep step) => this.Step == step;

    public void ShowTutorial()
    {
        Debug.Log(_step);

        if (IsToolTip(_step))
        {
            // Tooltip ���
            currentTooltip = AnalyzeTooltip(_step);
            if (currentTooltip.IsEnd)
                DisableToolTip();
            else
                EnableToolTip(currentTooltip);
        }
        else
        {
            // UI ���
            int indexToUI = AnalyzeUI(_step);
            isEnable = true;
            UI.TutorialActive(indexToUI);
        }
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

    private IEnumerator ClickCoolTime()
    {
        isCanClick = false;
        yield return new WaitForSeconds(RECLICK_TIME);
        isCanClick = true;
    }
}

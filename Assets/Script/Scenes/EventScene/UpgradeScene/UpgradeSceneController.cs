using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSceneController : MonoBehaviour
{
    private DeckUnit _unit;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject fall_background;

    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    [SerializeField] private GameObject _restoreFall_Btn;
    [SerializeField] private GameObject _ui_SelectMenu;

    private List<Script> _scripts;
    private UI_Conversation _conversationUI;
    private List<Upgrade> _upgradeList = new();

    private bool _isUpgradeFull = false;
    private bool _isNPCFall = false;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (GameManager.Data.GameData.npcQuest.upgradeQuest > 100)
        {
            background.SetActive(false);
            fall_background.SetActive(true);
            _isNPCFall = true;
        }
        else if (GameManager.Data.GameData.npcQuest.upgradeQuest > 100 * 3 / 4 )
        {
            //안개이미지 변경
        }
        else if (GameManager.Data.GameData.npcQuest.upgradeQuest > 100 / 2)
        {
            //안개이미지 변경
        }
        else if(GameManager.Data.GameData.npcQuest.upgradeQuest > 100 / 4)
        {
            //안개이미지 변경
        }

        if (!GameManager.OutGameData.IsUnlockedItem(2))
        {
            _restoreFall_Btn.SetActive(false);
        }

        _scripts = new ();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            _scripts = GameManager.Data.ScriptData["강화소_입장_최초"];
            GameManager.Data.GameData.isVisitUpgrade = true;
        }
        else
        {
            _scripts = GameManager.Data.ScriptData["강화소_입장"];
        }

        _conversationUI = GameManager.UI.ShowPopup<UI_Conversation>();
        _conversationUI.Init(_scripts);
        _conversationUI.ConversationEnded += OnConversationEnded;
    }

    // 업그레이드 할 유닛을 고릅니다.
    public void OnUpgradeUnitButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        _ui_SelectMenu.SetActive(false);
        UI_MyDeck ui = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ui.Init(false, OnSelectUpgrade, CUR_EVENT.UPGRADE, null);
        ui.SetEventMenu(_ui_SelectMenu);
    }

    // 교화를 풀 유닛을 고릅니다.
    public void OnReleaseUnitButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        _ui_SelectMenu.SetActive(false);
        UI_MyDeck ui = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ui.Init(false, OnSelectRelease, CUR_EVENT.RELEASE);
        ui.SetEventMenu(_ui_SelectMenu);
    }

    //대화하기 버튼
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
    }

    public void OnSelectUpgrade(DeckUnit unit)
    {
        _unit = unit;

        while (_upgradeList.Count < 3)
        {
            Upgrade upgrade = GameManager.Data.UpgradeController.GetRandomUpgrade();

            if (!_upgradeList.Contains(upgrade))
            {
                _upgradeList.Add(upgrade);
            }
        }

        if (_unit.DeckUnitUpgrade.Count == 2 || (_unit.DeckUnitUpgrade.Count == 3 && GameManager.OutGameData.IsUnlockedItem(12)))
        {
            GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().Init(this, _unit.DeckUnitUpgrade);
            _isUpgradeFull = true;
        }
        else
        {
            GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().Init(this, _upgradeList);
            _isUpgradeFull = false;
        }
    }

    public void OnSelectRelease(DeckUnit unit)
    {
        _unit = unit;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

        if (_unit.DeckUnitStat.FallCurrentCount > 0)
        {
            _unit.DeckUnitUpgradeStat.FallCurrentCount -= 1;
        }

        if (_isNPCFall && _unit.DeckUnitStat.FallCurrentCount > 0)
        {
            _unit.DeckUnitUpgradeStat.FallCurrentCount -= 1;
        }

        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(_unit);
        unitInfo.Init(null, CUR_EVENT.COMPLETE_RELEASE,OnQuitClick);
    }

    public void OnUpgradeSelect(int select)
    {
        GameManager.UI.CloseAllPopup();

        if (_isUpgradeFull)
        {
            _isUpgradeFull = false;

            UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
            unitInfo.SetUnit(_unit);
            unitInfo.Init(null, CUR_EVENT.UPGRADE_EXCEPTION);

            _unit.DeckUnitUpgrade.Remove(_unit.DeckUnitUpgrade[select]);
            GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().Init(this, _upgradeList);
        }
        else
        {
            _unit.DeckUnitUpgrade.Add(_upgradeList[select]);
            GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");

            UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
            unitInfo.SetUnit(_unit);
            unitInfo.Init(null, CUR_EVENT.COMPLETE_UPGRADE, OnQuitClick);

        }
    }

    public void OnQuitClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        StartCoroutine(QuitScene());
        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
        }
    }

    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        /*
        if (GameManager.Data.GameData.isVisitStigma == false)
        {
            GameManager.Data.GameData.isVisitStigma = true;
        }
        */

        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }

    private void OnConversationEnded()
    {
        _ui_SelectMenu.SetActive(true);
    }
}
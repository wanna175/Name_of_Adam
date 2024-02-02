using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSceneController : MonoBehaviour
{
    private DeckUnit _unit;

    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    [SerializeField] private GameObject _restoreFall_Btn;
    [SerializeField] private GameObject _ui_SelectMenu;
    
    private List<Script> _scripts;
    private UI_Conversation _conversationUI;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        if (!GameManager.OutGameData.IsUnlockedItem(2))
        {
            _restoreFall_Btn.SetActive(false);
        }

        _scripts = new ();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            _scripts = GameManager.Data.ScriptData["강화소_입장_최초"];
            GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
            _conversationUI = FindObjectOfType<UI_Conversation>();
            _conversationUI.ConversationEnded += OnConversationEnded;
            GameManager.Data.GameData.isVisitUpgrade = true;
        }
        else
        {
            _scripts = GameManager.Data.ScriptData["강화소_입장"];
            GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
            _conversationUI = FindObjectOfType<UI_Conversation>();
            _conversationUI.ConversationEnded += OnConversationEnded;
        }
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
        GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().Init(this);
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
        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(_unit);
        unitInfo.Init(null, CUR_EVENT.COMPLETE_RELEASE,OnQuitClick);
    }

    public void OnUpgradeSelect(int select)
    {
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

        _unit.DeckUnitUpgradeStat.CurrentUpgradeCount++;

        if (select == 1)
        {
            _unit.DeckUnitUpgrade.Add(GameManager.Data.UpgradeController.GetRandomUpgrade());
            //_unit.DeckUnitUpgradeStat.ATK += 5;
          // script.Init(GameManager.Data.ScriptData["강화소_공격력"], false);
        }
        else if (select == 2)
        {
            _unit.DeckUnitUpgradeStat.MaxHP += 15;
            _unit.DeckUnitUpgradeStat.CurrentHP += 15;
            //script.Init(GameManager.Data.ScriptData["강화소_체력"], false);
        }
        else if (select == 3)
        {
            _unit.DeckUnitUpgradeStat.SPD += 25;
           // script.Init(GameManager.Data.ScriptData["강화소_속도"], false);
        }
        else if (select == 4)
        {
            _unit.DeckUnitUpgradeStat.ManaCost -= 10;
            //script.Init(GameManager.Data.ScriptData["강화소_코스트"], false);
        }
        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        UI_UnitInfo _UnitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        _UnitInfo.SetUnit(_unit);
        _UnitInfo.Init(null, CUR_EVENT.COMPLETE_UPGRADE,OnQuitClick);
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
        if (GameManager.Data.GameData.isVisitStigma == false)
        {
            GameManager.Data.GameData.isVisitStigma = true;
        }

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
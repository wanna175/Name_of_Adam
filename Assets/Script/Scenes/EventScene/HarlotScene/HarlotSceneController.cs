using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HarlotSceneController : MonoBehaviour, StigmaInterface
{
    private readonly int[] enterDialogNums = { 3, 3, 3, 3, 3 };
    private readonly int[] exitDialogNums = { 1, 1, 1, 1, 1 };

    [SerializeField] private GameObject _normalBackground;
    [SerializeField] private GameObject _corruptBackground;
    [SerializeField] private List<GameObject> _fogImageList;

    [SerializeField] private GameObject _stigmataBestowalButton;
    [SerializeField] private GameObject _disabledStigmataBestowalButton;
    [SerializeField] private GameObject _apostleCreationButton;
    [SerializeField] private GameObject _disabledApostleCreationButton;
    [SerializeField] private GameObject _selectMenuUI;

    [SerializeField] private List<DeckUnit> _originUnits = null;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [SerializeField] private TextMeshProUGUI _revertUnitDarkEssenceText;
    [SerializeField] private TextMeshProUGUI _apostleCreationDarkEssenceText;
    [SerializeField] private TextMeshProUGUI _disabledApostleCreationDarkEssenceText;
    [SerializeField] private TextMeshProUGUI _stigmataBestowalDarkEssenceText;
    [SerializeField] private TextMeshProUGUI _disabledStigmataBestowalDarkEssenceText;
    private UI_Conversation _conversationUI;

    private List<Script> _scripts = new();

    private DeckUnit _stigmataBestowalUnit;
    private List<Stigma> _stigmataList = new();
    private List<DeckUnit> _revertUnits = new();

    private Stigma _preSelectedStigmata;
    private bool _isStigmataPreSet = false;

    private bool _isStigmataFull = false;
    private bool _isNPCFall = false;

    void Start()
    {
        Init();
    }

    private void Init()
    {

        Debug.Log($"횟수: {GameManager.Data.GameData.NpcQuest.DarkshopQuest}");

        int questLevel = Mathf.Min((int)(GameManager.Data.GameData.NpcQuest.DarkshopQuest / 7.5f), 4);

        if (GameManager.OutGameData.GetVisitDarkshop() == false && questLevel != 4)
        {
            _scripts = GameManager.Data.ScriptData["탕녀_입장_최초"];
            _descriptionText.SetText(GameManager.Locale.GetLocalizedScriptInfo(GameManager.Data.ScriptData["탕녀_선택_0"][0].script));
            _nameText.SetText(GameManager.Locale.GetLocalizedScriptName(GameManager.Data.ScriptData["탕녀_선택_0"][0].name));
        }
        else
        {
            _scripts = GameManager.Data.ScriptData[$"탕녀_입장_{25 * questLevel}_랜덤코드:{Random.Range(0, enterDialogNums[questLevel])}"];
            _descriptionText.SetText(GameManager.Locale.GetLocalizedScriptInfo(GameManager.Data.ScriptData[$"탕녀_선택_{25 * questLevel}"][0].script));
            _nameText.SetText(GameManager.Locale.GetLocalizedScriptName(GameManager.Data.ScriptData[$"탕녀_선택_{25 * questLevel}"][0].name));

            if (questLevel == 4)
            {
                _normalBackground.SetActive(false);
                _corruptBackground.SetActive(true);
                _isNPCFall = true;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            _fogImageList[i].gameObject.SetActive(questLevel > i);
        }

        GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
        _conversationUI = FindObjectOfType<UI_Conversation>();
        _conversationUI.ConversationEnded += OnConversationEnded;

        int current_DarkEssense = GameManager.Data.GameData.DarkEssence;

        if (!GameManager.OutGameData.IsUnlockedItem(5))
        {
            _apostleCreationButton.SetActive(false);
        }
        else if (current_DarkEssense < ((_isNPCFall) ? 8 : 10)) 
        {
            _disabledApostleCreationButton.SetActive(true);
            _apostleCreationButton.SetActive(false);
        }

        if (current_DarkEssense < ((_isNPCFall) ? 8 : 10))
        {
            _disabledStigmataBestowalButton.SetActive(true);
            _stigmataBestowalButton.SetActive(false);
        }
        SetMenuText(_isNPCFall);
    }

    //사도 연성 버튼 클릭
    public void OnApostleCreationButtonClick()
    {
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");

        DeckUnit originalUnit = new();
        originalUnit.Data = _originUnits[Random.Range(0, 3)].Data;

        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(originalUnit);
        unitInfo.Init(OnApostleSelect, CurrentEvent.COMPLETE_HAELOT, OnQuitClick);
    }

    public void OnApostleSelect(DeckUnit unit)
    {
        GameManager.Data.AddDeckUnit(unit);
        GameManager.Data.DarkEssenseChage(((_isNPCFall) ? -8 : -10));
        GameManager.Data.GameData.FallenUnits.Add(unit);
    }

    //유닛을 검은 정수로 환원하는 버튼
    public void OnRevertUnitButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        _revertUnits.Clear();

        UI_MyDeck myDeck = GameManager.UI.ShowPopup<UI_MyDeck>();
        myDeck.Init();
        myDeck.EventInit(OnSelectRevertUnit, CurrentEvent.Unit_Restoration_Select, _selectMenuUI, RestorationQuitClick);
    }

    public void OnSelectRevertUnit(DeckUnit unit)
    {
        if (!_revertUnits.Contains(unit))
            _revertUnits.Add(unit);
        else
            _revertUnits.Remove(unit);

        GameManager.UI.ClosePopup();
    }

    //타락 성흔 부여 버튼 클릭
    public void OnStigmataBestowalButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        UI_MyDeck myDeck = GameManager.UI.ShowPopup<UI_MyDeck>();
        myDeck.Init();
        myDeck.EventInit(OnSelectStigmataBestowalUnit, CurrentEvent.Stigmata_Select, _selectMenuUI);
    }

    public void UnitStigmataFull()
    {
        UI_StigmaSelectButtonPopup popup = GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>();
        popup.Init(null, true, _stigmataBestowalUnit.GetStigma(true));
        popup.EventInit(this, CurrentEvent.Stigmata_Full_Exception);

        _isStigmataFull = true;
    }

    public void OnSelectStigmataBestowalUnit(DeckUnit unit)
    {
        _stigmataBestowalUnit = unit;
        GameManager.Data.DarkEssenseChage(((_isNPCFall) ? -8 : -10));
        if (_stigmataBestowalUnit.GetStigmaCount() < _stigmataBestowalUnit.MaxStigmaCount)
        {
            ResetStigmataList(unit);

            UI_StigmaSelectButtonPopup popup = GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>();
            popup.Init(_stigmataBestowalUnit, false, _stigmataList);
            popup.EventInit(this, CurrentEvent.Stigmata_Select);
        }
        else
        {
            UnitStigmataFull();
        }

        //선 저장
        if (!GameManager.Data.Map.ClearTileID.Contains(GameManager.Data.Map.CurrentTileID))
        {
            GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        }
        GameManager.SaveManager.SaveGame();
    }

    private void BestowalStigmata(Stigma stigma)
    {
        if (_isStigmataPreSet)
        {
            _stigmataBestowalUnit.DeleteStigma(_preSelectedStigmata);
            _isStigmataPreSet = false;
        }
        _stigmataBestowalUnit.AddStigma(stigma);

        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
        unitInfo.SetUnit(_stigmataBestowalUnit);
        unitInfo.Init(null, CurrentEvent.Complate_Stigmata, OnQuitClick);
    }

    public void OnStigmataSelected(Stigma stigma)
    {
        if (_isStigmataFull)//스티그마 예외처리
        {
            _isStigmataFull = false;
            _stigmataBestowalUnit.DeleteStigma(stigma);
            GameManager.UI.CloseAllPopup();
            UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
            unitInfo.SetUnit(_stigmataBestowalUnit);

            ResetStigmataList(_stigmataBestowalUnit);
            unitInfo.Init(OnSelectStigmataBestowalUnit, CurrentEvent.Stigmata_Full_Exception);
        }
        else
        {
            BestowalStigmata(stigma);
        }
    }

    //대화하기 버튼을 클릭했을 경우
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
    }

    public List<Stigma> ResetStigmataList(DeckUnit stigmataTargetUnit)
    {
        _stigmataList.Clear();
        _stigmataList = GameManager.Data.StigmaController.GetRandomHarlotStigmaList(stigmataTargetUnit, 2);

        //선 적용된 성흔 리셋
        if (_isStigmataPreSet)
        {
            _stigmataBestowalUnit.DeleteStigma(_preSelectedStigmata);
        }

        _preSelectedStigmata = _stigmataList[Random.Range(0, _stigmataList.Count)];

        if (_stigmataBestowalUnit.GetStigmaCount() < _stigmataBestowalUnit.MaxStigmaCount)
        {
            _stigmataBestowalUnit.AddStigma(_preSelectedStigmata);
            GameManager.SaveManager.SaveGame();

            _isStigmataPreSet = true;
        }

        return _stigmataList;
    }

    public void RestorationQuitClick()
    {
        if (_revertUnits.Count == GameManager.Data.GetDeck().Count)
        {
            Debug.Log("유닛은 하나 이상 남겨야 합니다.");//여기다가 경고창 띄우면 될듯
            return;
        }
        else if (_revertUnits.Count != 0)
        {
            int cost = _isNPCFall ? 2 * _revertUnits.Count : _revertUnits.Count;
            GameManager.Data.DarkEssenseChage(cost);
            foreach (DeckUnit delunit in _revertUnits)
                GameManager.Data.RemoveDeckUnit(delunit);
        }

        GameManager.UI.ClosePopup();
        OnQuitClick();
    } 

    public void OnQuitClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        StartCoroutine(QuitScene());
    }

    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        if (GameManager.Data.GameData.IsVisitDarkShop == false)
        {
            GameManager.Data.GameData.IsVisitDarkShop = true;
        }
        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        if (GameManager.OutGameData.GetVisitDarkshop()==false)
        {
            GameManager.OutGameData.SetVisitDarkshop(true);
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장_최초"], false);
        }
        else 
        {
            int questLevel = (int)(GameManager.Data.GameData.NpcQuest.DarkshopQuest / 7.5f);
            if (questLevel > 4) questLevel = 4;
            quitScript.Init(GameManager.Data.ScriptData[$"탕녀_퇴장_{25 * questLevel}_랜덤코드:{Random.Range(0, exitDialogNums[questLevel])}"], false);
        }
        
        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }

    private void SetMenuText(bool isNpcfall)
    {
        if (isNpcfall)
        {
            _revertUnitDarkEssenceText.text = "2";
            _stigmataBestowalDarkEssenceText.text = "8";
            _apostleCreationDarkEssenceText.text = "8";

            _disabledApostleCreationDarkEssenceText.text = "8";
            _disabledStigmataBestowalDarkEssenceText.text = "8";

        }
        else
        {
            _revertUnitDarkEssenceText.text = "1";
            _stigmataBestowalDarkEssenceText.text = "10";
            _apostleCreationDarkEssenceText.text = "10";

            _disabledApostleCreationDarkEssenceText.text = "10";
            _disabledStigmataBestowalDarkEssenceText.text = "10";
        }
    }

    private void OnConversationEnded()
    {
        _selectMenuUI.SetActive(true);
    }
}

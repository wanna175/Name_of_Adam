using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarlotSceneController : MonoBehaviour,StigmaInterface
{
    private readonly int[] enterDialogNums = { 3, 3, 3, 3, 3 };
    private readonly int[] exitDialogNums = { 1, 1, 1, 1, 1 };

    private DeckUnit _stigmatizeUnit;
    private List<Stigma> stigma; //타락낙인 저장하는 곳
    private List<DeckUnit> _RestorationUnits;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject fall_background;

    [SerializeField] private GameObject _SelectStigmaButton;
    [SerializeField] private GameObject _SelectStigmaButton_disabled;
    [SerializeField] private GameObject _getOriginUnitButton;
    [SerializeField] private GameObject _getOriginUnitButton_disabled;
    [SerializeField] private GameObject _ui_SelectMenu;
    [SerializeField] private List<DeckUnit> _originUnits = null;

    List<Script> scripts = null;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    private bool _isStigmaFull;

    private UI_Conversation uiConversation;

    private bool isNPCFall = false;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        scripts = new List<Script>();
        _isStigmaFull = false;
        _RestorationUnits = new List<DeckUnit>();
        if (GameManager.OutGameData.getVisitDarkshop() == false)
        {
            scripts = GameManager.Data.ScriptData["탕녀_입장_최초"];
            //GameManager.OutGameData.setVisitDarkshop(true);
        }
        else
        {
            int questLevel = (int)(GameManager.Data.GameData.NpcQuest.StigmaQuest / 7.5f);
            if (questLevel > 4) questLevel = 4;
            scripts = GameManager.Data.ScriptData[$"탕녀_입장_{25 * questLevel}_랜덤코드:{Random.Range(0, enterDialogNums[questLevel])}"];

            if (questLevel == 4)
            {   
                background.SetActive(false);
                fall_background.SetActive(true);
                this.isNPCFall = true;
            }
            else if (questLevel > 0)
            {
                // 안개이미지 변경
            }
        }

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
        uiConversation = FindObjectOfType<UI_Conversation>();
        uiConversation.ConversationEnded += OnConversationEnded;

        stigma = GameManager.Data.StigmaController.get_harlotStigmaList;

        int current_DarkEssense = GameManager.Data.DarkEssense;

        if (!GameManager.OutGameData.IsUnlockedItem(5))
        {
            _getOriginUnitButton.SetActive(false);
        }
        else if (current_DarkEssense < ((isNPCFall) ? 8 : 10)) 
        {
            _getOriginUnitButton_disabled.SetActive(true);
            _getOriginUnitButton.SetActive(false);
        }

        if (current_DarkEssense < ((isNPCFall) ? 5 : 7))
        {
            _SelectStigmaButton_disabled.SetActive(true);
            _SelectStigmaButton.SetActive(false);
        }

        /*
        if(GameManager.Data.GameData)
        {
            _forbiddenButton.gameObject.SetActive(true);
        }
        else
        {
            _forbiddenButton.gameObject.SetActive(false);
        }
        */
    }
    //오리지날 유닛 연성하기
    public void OnMakeOriginUnitClick()
    {
        //연출하기 애니매이션 끝나면 
        //Debug.Log(GameManager.Data.GameData.DeckUnits);
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        int unitidx = Random.Range(0, 3);
        UI_UnitInfo _ui = GameManager.UI.ShowPopup<UI_UnitInfo>();
        _ui.SetUnit(_originUnits[unitidx]);
        _ui.Init(OnSelectMakeUnit,CUR_EVENT.COMPLETE_HAELOT,OnQuitClick);

    }
    public void OnSelectMakeUnit(DeckUnit unit)
    {
        GameManager.Data.AddDeckUnit(unit);
        GameManager.Data.DarkEssenseChage(((isNPCFall) ? -8 : -10));
    }
    //유닛을 검은 정수로 환원하는 버튼
    public void OnUnitRestorationClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_MyDeck _ui = GameManager.UI.ShowPopup<UI_MyDeck>();
        _ui.Init(false, OnSelectRestoration, CUR_EVENT.HARLOT_RESTORATION, OnQuitClick);
        _ui.SetEventMenu(_ui_SelectMenu);
    }
    public void OnSelectRestoration(DeckUnit unit)
    {
        if (!_RestorationUnits.Contains(unit))
            _RestorationUnits.Add(unit);
        else
            _RestorationUnits.Remove(unit);

        GameManager.UI.ClosePopup();
    }
    // 유닛 선택 후 타락 관련 낙인 부여 버튼
    public void OnStigmaButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_MyDeck _ui = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        _ui.Init(false, OnSelectStigmatization, CUR_EVENT.STIGMA);
        _ui.SetEventMenu(_ui_SelectMenu);
    }
    public void IsStigmaFull()
    {
        Debug.Log("스티그마 꽉 찼을 때 예외처리");
        _isStigmaFull = true;
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _stigmatizeUnit.GetStigma(true), 0, null, this);
    }

    public void OnSelectStigmatization(DeckUnit unit)
    {
        _stigmatizeUnit = unit;

        if (_stigmatizeUnit.GetStigmaCount() < _stigmatizeUnit._maxStigmaCount)
        {
            Debug.Log("유닛이 스티그마를 더 받을 수 잇는 상태입니다.");
            //GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, null, 3, null, this);
            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, stigma,2,null,this);
        }
        else
        {
            Debug.Log("유닛 스티그마 더 받을 수 없으니 하나를 선택하여 지워야 합니다.");
            IsStigmaFull();
        }
        GameManager.Data.DarkEssenseChage(((isNPCFall) ? -5 : -7));
    }
    private void SetUnitStigma(Stigma stigma)
    {
        _stigmatizeUnit.AddStigma(stigma);

        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>();
        ui.SetUnit(_stigmatizeUnit);
        ui.Init(null, CUR_EVENT.COMPLETE_STIGMA, OnQuitClick);
    }
    public void OnStigmaSelected(Stigma stigma)
    {
        if (_isStigmaFull)//스티그마 예외처리
        {
            Debug.Log("스티그마 꽉차있서요");
            _isStigmaFull = false;
            _stigmatizeUnit.DeleteStigma(stigma);
            GameManager.UI.CloseAllPopup();
            UI_UnitInfo unitInfo = GameManager.UI.ShowPopup<UI_UnitInfo>();
            unitInfo.SetUnit(_stigmatizeUnit);
            unitInfo.Init(OnSelectStigmatization, CUR_EVENT.STIGMA_EXCEPTION);
            return;
        }
        Debug.Log("타락 낙인 부여일때");
        SetUnitStigma(stigma);
    }
    //대화하기 버튼을 클릭했을 경우
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }
    //나가기 버튼을 클릭했을 경우
    public void OnQuitClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        if (_RestorationUnits.Count == GameManager.Data.GetDeck().Count)
        {
            Debug.Log("유닛은 하나 이상 남겨야 합니다.");
            return;
        }
        else if (_RestorationUnits.Count != 0)
        {
            int cost = isNPCFall ? 2 * _RestorationUnits.Count : _RestorationUnits.Count;
            GameManager.Data.DarkEssenseChage(cost);
            foreach(DeckUnit delunit in _RestorationUnits)
                GameManager.Data.RemoveDeckUnit(delunit);
        }

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

        if (GameManager.OutGameData.getVisitDarkshop()==false)
        {
            GameManager.OutGameData.setVisitDarkshop(true);
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장_최초"], false);
        }
        else 
        {
            int questLevel = (int)(GameManager.Data.GameData.NpcQuest.StigmaQuest / 7.5f);
            if (questLevel > 4) questLevel = 4;
            quitScript.Init(GameManager.Data.ScriptData[$"탕녀_퇴장_{25 * questLevel}_랜덤코드:{Random.Range(0, exitDialogNums[questLevel])}"], false);
        }
        
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

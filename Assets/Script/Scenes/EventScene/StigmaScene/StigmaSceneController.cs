using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StigmaSceneController : MonoBehaviour,StigmaInterface
{
    private DeckUnit _givestigmatizeUnit;
    private DeckUnit _stigmatizeUnit;
    private List<Script> _scripts;

    private bool _isStigmaFull = false;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject fall_background;

    [SerializeField] private GameObject _stigma_transfer_btn = null;
    [SerializeField] private GameObject _stigma_transfer_btn_disabled;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    [SerializeField] private GameObject _ui_SelectMenu;

    private UI_Conversation uiConversation;
    private Stigma _giveStigma = null;

    private bool isNPCFall = false;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        _scripts = new();
        _giveStigma = null;
        _isStigmaFull = false;
        Debug.Log("dddddddd : " + GameManager.Data.GameData.NpcQuest.StigmaQuest);

        //옮길 낙인유닛이 없다면 선택지가 안 뜨게
        bool isStigmaEmpty = true;
        List<DeckUnit> du = GameManager.Data.GetDeck();
        foreach(DeckUnit unit in du)
        {
            if (unit.GetStigma(true).Count != 0)
            {
                isStigmaEmpty = false;
                break;
            }
        }

        if (!GameManager.OutGameData.IsUnlockedItem(11))
        {
            _stigma_transfer_btn.SetActive(false);
        }
        else if (isStigmaEmpty)
        {
            _stigma_transfer_btn.SetActive(false);
            _stigma_transfer_btn_disabled.SetActive(true);
        }
        Debug.Log("init GameManager.OutGameData.getVisitStigma() : " + GameManager.OutGameData.getVisitStigma());
        if (GameManager.OutGameData.getVisitStigma() == false)
        {
            //GameManager.OutGameData.setVisitStigma(true);
            _scripts = GameManager.Data.ScriptData["낙인소_입장_최초"];
            Debug.Log("낙인소 입장 최초");
        }
        else
        {
            Debug.Log("낙인소 입장 햇슴");
            if (GameManager.Data.GameData.NpcQuest.StigmaQuest >=50)
            {
                _scripts = GameManager.Data.ScriptData["타락_낙인소_입장"];
                background.SetActive(false);
                fall_background.SetActive(true);
                this.isNPCFall = true;
            }
            else if (GameManager.Data.GameData.NpcQuest.StigmaQuest >= 50 * 3 / 4)
            {
                _scripts = GameManager.Data.ScriptData["타락_낙인소_입장_50"];
                //안개이미지 변경
            }
            else if (GameManager.Data.GameData.NpcQuest.StigmaQuest >= 50 / 2)
            {
                _scripts = GameManager.Data.ScriptData["타락_낙인소_입장_50"];
                //안개이미지 변경
            }
            else if (GameManager.Data.GameData.NpcQuest.StigmaQuest >= 50 / 4)
            {
                _scripts = GameManager.Data.ScriptData["낙인소_입장"];
                //안개이미지 변경
            }
            else
                _scripts = GameManager.Data.ScriptData["낙인소_입장"];
        }

        GameManager.UI.ShowPopup<UI_Conversation>().Init(_scripts);
        uiConversation = FindObjectOfType<UI_Conversation>();
        uiConversation.ConversationEnded += OnConversationEnded;
    }

    // 낙인 부여를 선택했을 시
    public void OnStigmaUnitButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        this._ui_SelectMenu.SetActive(false);
        UI_MyDeck ud = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ud.Init(false, OnSelectStigmatization, CUR_EVENT.STIGMA);
        ud.SetEventMenu(this._ui_SelectMenu);
    }

    //낙인을 받는 유닛을 고르는 함수
    public void OnSelectStigmaTargetUnit()
    {
        GameManager.UI.CloseAllPopup();
        this._ui_SelectMenu.SetActive(false);
        UI_MyDeck ud = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ud.Init(false, OnSelectStigmatransfertarget, CUR_EVENT.RECEIVE_STIGMA);
        ud.SetEventMenu(this._ui_SelectMenu);
    }

    // 낙인을 주는 유닛을 고르는 함수,낙인 이동을 눌렀을 경우에
    public void OnStigmaGiveUnitButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        this._ui_SelectMenu.SetActive(false);
        UI_MyDeck ud = GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ud.Init(false, OnSelectStigmatransfergiver, CUR_EVENT.GIVE_STIGMA);
        ud.SetEventMenu(this._ui_SelectMenu);
    }

    // 낙인소 나가기 
    public void OnQuitClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        StartCoroutine(QuitScene());

    }

    // 낙인 이동 실행시킬 함수
    public void StigmaTransfer()
    {
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _givestigmatizeUnit.GetStigma());
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

        if (_stigmatizeUnit.GetStigmaCount()< _stigmatizeUnit._maxStigmaCount)
        {
            Debug.Log("유닛이 스티그마를 더 받을 수 잇는 상태입니다.");
            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, null, 3, null, this);
        }
        else
        {
            Debug.Log("유닛 스티그마 더 받을 수 없으니 하나를 선택하여 지워야 합니다.");
            IsStigmaFull();
        }
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
        ui.Init(null, CUR_EVENT.COMPLETE_STIGMA,OnQuitClick);
    }

    public void OnSelectStigmatransfertarget(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        if (_stigmatizeUnit.GetStigmaCount() <_stigmatizeUnit._maxStigmaCount)
        {
            Debug.Log("유닛이 스티그마를 더 받을 수 잇는 상태입니다.");
            SetUnitStigma(_giveStigma);

        }
        else
        {
            Debug.Log("유닛 스티그마 더 받을 수 없으니 하나를 선택하여 지워야 합니다.");
            IsStigmaFull();
        }
    }

    public void OnSelectStigmatransfergiver(DeckUnit unit)
    {
        _givestigmatizeUnit = unit;
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _givestigmatizeUnit.GetStigma(true),0,null,this);
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

            if (_givestigmatizeUnit == null)
                unitInfo.Init(OnSelectStigmatization, CUR_EVENT.STIGMA_EXCEPTION);
            else
                unitInfo.Init(OnSelectStigmatransfertarget, CUR_EVENT.STIGMA_EXCEPTION);
            return;
        }
        if (_givestigmatizeUnit == null) 
        {//낙인 부여일때
            SetUnitStigma(stigma);
            GameManager.Data.GameData.NpcQuest.StigmaQuest++;
        }
        else//낙인 이동일때
        {
            _giveStigma = stigma;
            if(!isNPCFall)
                GameManager.Data.RemoveDeckUnit(_givestigmatizeUnit);
            else
                _givestigmatizeUnit.DeleteStigma(stigma);
            GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
            OnSelectStigmaTargetUnit();
        }
    }
    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        
        if (GameManager.Data.GameData.IsVisitStigma == false)
        {
            GameManager.Data.GameData.IsVisitStigma = true;
        }
        

        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        if (GameManager.OutGameData.getVisitStigma() == false)
        {
            GameManager.OutGameData.setVisitStigma(true);
            Debug.Log("GameManager.OutGameData.getVisitStigma() : " + GameManager.OutGameData.getVisitStigma());
            quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장_최초"], false);
        }
        else
        {
            if (GameManager.Data.GameData.NpcQuest.StigmaQuest > 50)
            {
                quitScript.Init(GameManager.Data.ScriptData["타락_낙인소_퇴장_100"], false);
            }
            else if (GameManager.Data.GameData.NpcQuest.StigmaQuest > 50 / 2)
            {
                quitScript.Init(GameManager.Data.ScriptData["타락_낙인소_퇴장_50"], false);
            }
            else
                quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장"], false);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StigmaSceneController : MonoBehaviour
{
    private DeckUnit _givestigmatizeUnit;
    private DeckUnit _stigmatizeUnit;
    private List<Script> scripts;
    
    private bool isStigmaSelect;
    private bool isGiveStigmaSelect;
    private bool willTargetSelect;
    private bool isTargetStigmaSelect;
    [SerializeField] private GameObject _menu_select = null;
    [SerializeField] private GameObject _stigma_transfer_btn = null;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    public static Stigma _giveStigma = null;

    public static bool isEnd = false;
    void Start()
    {
        Init();
    }
    private void Update()
    {
        if (isStigmaSelect && _stigmatizeUnit != null)
            OnStigmaButtonClick();
        if (isGiveStigmaSelect && _givestigmatizeUnit != null)
            StigmaTransfer();
        if (willTargetSelect&&_giveStigma != null) {
            //_givestigmatizeUnit을 삭제해야됨.
            OnStigmaTargetUnitButtonClick();
        }
        if (isTargetStigmaSelect&&_stigmatizeUnit != null)
        {
            isTargetStigmaSelect = false;
            _stigmatizeUnit.AddStigma(_giveStigma);
            UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>();
            ui.SetUnit(_stigmatizeUnit);
            ui.Init(null, (int)CUR_EVENT.COMPLETE_STIGMA);
        }
        if (isEnd)
        {
            isEnd = false;
            //script.gameObject.SetActive(true);
            StartCoroutine(QuitScene());
        }
    }

    private void Init()
    {
        isEnd = false;
        isStigmaSelect = false;
        isGiveStigmaSelect = false;
        willTargetSelect = true;
        isTargetStigmaSelect = false;
        scripts = new ();
        _giveStigma = null;
        UI_MyDeck ud = new ();
        if (ud.DeckCount == 0)
            _stigma_transfer_btn.SetActive(false);

        if (GameManager.Data.GameData.isVisitUpgrade == false)
            scripts = GameManager.Data.ScriptData["낙인소_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["낙인소_입장"];

    }

    //대화하기 버튼을 클릭했을 경우
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }
    // 낙인 부여를 선택했을 시
    public void OnStigmaUnitButtonClick()
    {
        isStigmaSelect = true;
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectStigmatization,(int)CUR_EVENT.STIGMA);
    }


    public void OnStigmaTargetUnitButtonClick()
    {
        willTargetSelect = false;
        isTargetStigmaSelect = true;
        GameManager.UI.CloseAllPopup();
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectStigmatransfertarget,(int)CUR_EVENT.RECEIVE_STIGMA);
    }

    // 낙인을 주는 유닛을 고르는 함수,낙인 이동을 눌렀을 경우에
    public void OnStigmaGiveUnitButtonClick()
    {
        isGiveStigmaSelect = true;
        UI_MyDeck ud =GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ud.Init(false, OnSelectStigmatransfergiver, (int)CUR_EVENT.GIVE_STIGMA);
        if (ud.DeckCount == 0)
        {
            isGiveStigmaSelect = false;
            Debug.Log("낙인 이동할 유닛이 없습니다. 나중에 어떻게 처리할 지 말해봐야 할듯");
            GameManager.UI.ClosePopup();
        }
        else
            _menu_select.SetActive(false);
    }

    // 낙인 선택지가 뜨는 함수
    public void OnStigmaButtonClick()
    {
        isStigmaSelect = false;
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, null, 3, null, this);
    }

    // 낙인소 나가기 
    public void OnQuitClick()
    {
        StartCoroutine(QuitScene());
    }

    // 낙인 이동 실행시킬 함수
    public void StigmaTransfer()
    {
        isGiveStigmaSelect = false;
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _givestigmatizeUnit.GetStigma());
        willTargetSelect = true;
    }
    public void OnSelectStigmatization(DeckUnit unit)
    {
        //_unitImage.gameObject.SetActive(true);
        _stigmatizeUnit = unit;
        //_unitImage.sprite = unit.Data.Image;
        //_unitImage.color = Color.white;
        
        //GameManager.UI.ClosePopup();
        //GameManager.UI.ClosePopup();
    }

    public void OnSelectStigmatransfertarget(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        //_targetunitImage.sprite = unit.Data.Image;
        //_targetunitImage.color = Color.white;

        //GameManager.UI.ClosePopup();
        //GameManager.UI.ClosePopup();
    }

    public void OnSelectStigmatransfergiver(DeckUnit unit)
    {
        _givestigmatizeUnit = unit;
       //_giveunitImage.sprite = unit.Data.Image;
        //_giveunitImage.color = Color.white;

        //GameManager.UI.ClosePopup();
        //GameManager.UI.ClosePopup();
    }


    public void OnStigmaSelected(Stigma stigma)
    {
        _stigmatizeUnit.AddStigma(stigma);

        GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>();
        ui.SetUnit(_stigmatizeUnit);
        ui.Init(null, (int)CUR_EVENT.COMPLETE_STIGMA);
        
        /*
        GameObject go = GameManager.VisualEffect.StartVisualEffect(
            Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UpgradeEffect"),
            _giveunitImage.transform.position + new Vector3(2f, -0.5f, 0));
        */
        //UI_Conversation script = GameManager.UI.ShowPopup<UI_Conversation>();
        //string scriptKey = "낙인소_" + stigma.Name;
        //script.Init(GameManager.Data.ScriptData[scriptKey], false);
        //StartCoroutine(QuitScene(script));
    }

    public void AddStigmaScript(Stigma stigma)
    {
        
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



        if (GameManager.Data.GameData.isVisitStigma == false)
        {
            GameManager.Data.GameData.isVisitStigma = true;
            quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StigmaSceneController : MonoBehaviour
{
    private DeckUnit _givestigmatizeUnit;
    private DeckUnit _stigmatizeUnit;
    private List<Script> scripts;
    
    
    [SerializeField] private GameObject _stigma_transfer_btn = null;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    private Stigma _giveStigma = null;

    void Start()
    {
        Init();
    }
    private void Init()
    {
        scripts = new ();
        _giveStigma = null;
        

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
        if (isStigmaEmpty)
        {
            _stigma_transfer_btn.gameObject.SetActive(false);
        }

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
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectStigmatization,(int)CUR_EVENT.STIGMA);
    }

    //낙인을 받는 유닛을 고르는 함수
    public void OnSelectStigmaTargetUnit()
    {
        GameManager.UI.CloseAllPopup();
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectStigmatransfertarget, (int)CUR_EVENT.RECEIVE_STIGMA);
    }

    // 낙인을 주는 유닛을 고르는 함수,낙인 이동을 눌렀을 경우에
    public void OnStigmaGiveUnitButtonClick()
    {
        UI_MyDeck ud =GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        ud.Init(false, OnSelectStigmatransfergiver, (int)CUR_EVENT.GIVE_STIGMA);
    }

    // 낙인소 나가기 
    public void OnQuitClick()
    {
        StartCoroutine(QuitScene());
    }

    // 낙인 이동 실행시킬 함수
    public void StigmaTransfer()
    {
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _givestigmatizeUnit.GetStigma());
    }
    public void OnSelectStigmatization(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        if (_stigmatizeUnit._stigmaCount < _stigmatizeUnit._maxStigmaCount)
        {
            Debug.Log("유닛이 스티그마를 더 받을 수 잇는 상태입니다.");
            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, null, 3, null, this);
        }
        else
        {
            Debug.Log("유닛 스티그마 더 받을 수 없으니 하나를 선택하여 지워야 합니다.");
            //TODO:낙인이 넘어갔을 때 예외처리
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
        ui.Init(null, (int)CUR_EVENT.COMPLETE_STIGMA,OnQuitClick);
    }
    public void OnSelectStigmatransfertarget(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        if (_stigmatizeUnit._stigmaCount < _stigmatizeUnit._maxStigmaCount)
        {
            Debug.Log("유닛이 스티그마를 더 받을 수 잇는 상태입니다.");
            SetUnitStigma(_giveStigma);
        }
        else
        {
            Debug.Log("유닛 스티그마 더 받을 수 없으니 하나를 선택하여 지워야 합니다.");
            //TODO:낙인이 넘어갔을 때 예외처리
        }
    }

    public void OnSelectStigmatransfergiver(DeckUnit unit)
    {
        _givestigmatizeUnit = unit;
        GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(null, _givestigmatizeUnit.GetStigma(true),0,AfterSelectGiveUnit,this);
    }
    public void AfterSelectGiveUnit()
    {
        Debug.Log("여기까지 오나요ㅕ???");
    }

    public void OnStigmaSelected(Stigma stigma)
    {
        if (_givestigmatizeUnit==null) {//낙인 부여일때
            Debug.Log("낙인 부여일때");
            SetUnitStigma(stigma);
        }
        else//낙인 이동일때
        {
            Debug.Log("낙인 이동일때");
            _giveStigma = stigma;
            GameManager.Data.RemoveDeckUnit(_givestigmatizeUnit);
            GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
            Debug.Log("받을 스티그마: " + _giveStigma.Description);
            OnSelectStigmaTargetUnit();
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
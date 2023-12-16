using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarlotSceneController : MonoBehaviour,StigmaInterface
{
    private DeckUnit _stigmatizeUnit;
    private Stigma stigma;
    [SerializeField] private GameObject _SelectStigmaButton = null;
    [SerializeField] private GameObject _getOriginUnitButton = null;

    List<Script> scripts = null;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼
    private bool _isStigmaFull;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        scripts = new List<Script>();
        _isStigmaFull = false;
        /*if (GameManager.Data.GameData.isVisitHarlot == false)
            scripts = GameManager.Data.ScriptData["탕녀_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["탕녀_입장"];
        */
        //GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        stigma = GameManager.Data.StigmaController.GetHarlotStigmas();

        Debug.Log(stigma.Name);
        Debug.Log("검은 정수: " + GameManager.Data.DarkEssense);
        int current_DarkEssense = GameManager.Data.DarkEssense;
        if (current_DarkEssense < 12)
            _getOriginUnitButton.SetActive(false);
        if (current_DarkEssense < 1)
            _SelectStigmaButton.SetActive(false);
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
    //유닛을 검은 정수로 환원하는 버튼
    public void OnUnitRestorationClick()
    {
        //GameManager.UI.ShowPopup<UI_MyDeck>().Init(false,)
    }

    // 유닛 선택 후 타락 관련 낙인 부여 버튼
    public void OnStigmaButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelectStigmatization, CUR_EVENT.STIGMA);
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
            List<Stigma> stigmata = new();
            stigmata.Add(stigma);
            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, stigmata,0,null,this);
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
        StartCoroutine(QuitScene());
    }
    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();



        /*if (GameManager.Data.GameData.isVisitHarlot == false)
        {
            GameManager.Data.GameData.isVisitHarlot = true;
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장"], false);
        */
        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }
}

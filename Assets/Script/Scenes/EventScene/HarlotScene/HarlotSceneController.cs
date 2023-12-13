using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarlotSceneController : MonoBehaviour
{
    private DeckUnit _stigmatizeUnit;
    private Stigma stigma;
    [SerializeField] private GameObject _SelectStigmaButton = null;
    [SerializeField] private GameObject _getOriginUnitButton = null;

    List<Script> scripts = null;
    [SerializeField] private Button _forbiddenButton; // 접근 금지 버튼

    void Start()
    {
        Init();
    }

    private void Init()
    {
        scripts = new List<Script>();

        if (GameManager.Data.GameData.isVisitHarlot == false)
            scripts = GameManager.Data.ScriptData["탕녀_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["탕녀_입장"];

        //GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        stigma = GameManager.Data.StigmaController.GetRandomStigma(GameManager.Data.GetProbability());
        //랜덤으로 타락스티그마를 받아오는 건가?? 그럼 타락스티그마랑 스티그마랑 같은 종류인가?

        Debug.Log(stigma.Name);
        int current_DarkEssense = GameManager.Data.DarkEssense;
        if (current_DarkEssense < 12)
            _getOriginUnitButton.SetActive(false);
        if (current_DarkEssense < 7)
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
        if (_stigmatizeUnit != null)
        {

            List<Stigma> stigmata = new();

            stigmata.Add(stigma);

            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, stigmata);
        }
    }
    //대화하기 버튼을 클릭했을 경우
    public void OnConversationButtonClick()
    {
        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }
    //나가기 버튼을 클릭했을 경우
    public void ClickQuit()
    {
        StartCoroutine(QuitScene());
    }

    private IEnumerator QuitScene(UI_Conversation eventScript = null)
    {
        if (eventScript != null)
            yield return StartCoroutine(eventScript.PrintScript());

        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();



        if (GameManager.Data.GameData.isVisitHarlot == false)
        {
            GameManager.Data.GameData.isVisitHarlot = true;
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["탕녀_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        GameManager.Data.Map.ClearTileID.Add(GameManager.Data.Map.CurrentTileID);
        GameManager.SaveManager.SaveGame();
        SceneChanger.SceneChange("StageSelectScene");
    }
}

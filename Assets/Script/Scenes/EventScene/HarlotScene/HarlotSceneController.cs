using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarlotSceneController : MonoBehaviour
{
    private DeckUnit _stigmatizeUnit;
    private Passive stigma;
    [SerializeField] private Image _unitImage; // 낙인 부여 유닛
    [SerializeField] private Image _stigmaImage; // 타락 낙인 이미지
    [SerializeField] private GameObject _menuChoose;
    [SerializeField] private GameObject _stigmazation;
    [SerializeField] private GameObject _getEliteUnit;



    void Start()
    {
        Init();
    }

    private void Init()
    {
        //_stigmaImage = GameManager.Resource.Load<Image>("");
        _menuChoose.SetActive(true);
        _stigmazation.SetActive(false);
        _getEliteUnit.SetActive(false);

        List<Script> scripts = new List<Script>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
            scripts = GameManager.Data.ScriptData["낙인소_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["낙인소_입장"];

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        PassiveManager passiveManager = GameManager.Data.Passive;

        stigma = GameManager.Data.Passive.GetRandomPassive();

        Debug.Log(stigma.GetName());
        //_stigmaImage = GameManager.Resource.Load<Image>("" + stigma.GetName());

    }

    public void ClickStigmatization()
    {
        _menuChoose.SetActive(false);

        _stigmazation.SetActive(true);


    }

    public void ClickGetEliteUnit()
    {
        _menuChoose.SetActive(false);
        _getEliteUnit.SetActive(true);

        // 유닛 얻는 내용

    }


    // 유닛 고르기 버튼
    public void ClickUnitSelectBTN()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelect);
    }

    // 유닛 선택 후 타락 관련 낙인 부여 버튼
    public void OnStigmaButtonClick()
    {
        if (_stigmatizeUnit != null)
        {

            List<Passive> stigmata = new List<Passive>();

            stigmata.Add(stigma);

            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, stigmata);
        }
    }



    public void OnSelect(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        _unitImage.sprite = unit.Data.Image;
        _unitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
    }

    

    public void OnStigmaSelect(Passive stigma)
    {
        _stigmatizeUnit.AddStigma(stigma);
        GameManager.UI.ClosePopup();
        AddStigamScript(stigma);
    }

    public void AddStigamScript(Passive stigma)
    {
        UI_Conversation script = GameManager.UI.ShowPopup<UI_Conversation>();
        string scriptKey = "낙인소_" + stigma.GetName();
        script.Init(GameManager.Data.ScriptData[scriptKey], false);
        StartCoroutine(QuitScene(script));
    }

    public void ClickQuit()
    {
        StartCoroutine(QuitScene());
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

        quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        SceneChanger.SceneChange("StageSelectScene");
    }
}

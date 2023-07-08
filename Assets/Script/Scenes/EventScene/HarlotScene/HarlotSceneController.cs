using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarlotSceneController : MonoBehaviour
{
    private DeckUnit _stigmatizeUnit;

    [SerializeField] private Image _unitImage; // ³«ÀÎ ºÎ¿© À¯´Ö
    [SerializeField] private Image _stigmaImage; // Å¸¶ô ³«ÀÎ ÀÌ¹ÌÁö
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
            scripts = GameManager.Data.ScriptData["³«ÀÎ¼Ò_ÀÔÀå_ÃÖÃÊ"];
        else
            scripts = GameManager.Data.ScriptData["³«ÀÎ¼Ò_ÀÔÀå"];

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        PassiveManager passiveManager = GameManager.Data.Passive;
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

        // À¯´Ö ¾ò´Â ³»¿ë

    }



    public void ClickUnitSelectBTN()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, OnSelect);
    }


    public void OnSelect(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        _unitImage.sprite = unit.Data.Image;
        _unitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
    }

    public void OnStigmaButtonClick()
    {
        if (_stigmatizeUnit != null)
        {
            GameManager.UI.ShowPopup<UI_StigmaSelectButtonPopup>().Init(_stigmatizeUnit, 3);
        }
    }

    public void OnStigmaSelect(Passive stigma)
    {
        _stigmatizeUnit.AddStigma(stigma);
        GameManager.UI.ClosePopup();
        AddStigamScript(stigma);
        //StartCoroutine(QuitScene());
        
        //OnQuitClick();
    }

    public void AddStigamScript(Passive stigma)
    {
        UI_Conversation script = GameManager.UI.ShowPopup<UI_Conversation>();
        string scriptKey = "³«ÀÎ¼Ò_" + stigma.GetName();
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

        quitScript.Init(GameManager.Data.ScriptData["³«ÀÎ¼Ò_ÅðÀå"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        SceneChanger.SceneChange("StageSelectScene");
    }
}

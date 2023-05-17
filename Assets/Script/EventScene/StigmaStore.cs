using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StigmaStore : Selectable
{
    private DeckUnit _stigmatizeUnit;

    [SerializeField] private Image _unitImage;

    List<Passive> stigmaList = new();

    void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Script> scripts = new List<Script>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
            scripts = GameManager.Data.ScriptData["낙인소_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["낙인소_입장"];

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        PassiveManager passiveManager = GameManager.Data.Passive;
        stigmaList.Add(passiveManager.GetRandomPassive());

        while (stigmaList.Count < 3)
        {
            Passive tempPassive = passiveManager.GetRandomPassive();
            
            if (stigmaList.Contains(tempPassive))
                continue;
            
            stigmaList.Add(tempPassive);
        }
    }

    public void OnStigmaUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, true, this);
    }

    public override void OnSelect(DeckUnit unit)
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
            GameManager.UI.ShowPopup<UI_StigmaSelectButton>().init(this, stigmaList);
        }
    }

    //public void OnStigmaSelect(int select) 
    //{
    //    if (select == 1)
    //    {
    //        _stigmatizeUnit.AddStigma(stigmaList[0]);
    //        GameManager.UI.ClosePopup();
    //        AddStigamScript(stigmaList[0]);
    //    }
    //    else if (select == 2)
    //    {
    //        _stigmatizeUnit.AddStigma(stigmaList[1]);
    //        GameManager.UI.ClosePopup();
    //        AddStigamScript(stigmaList[1]);
    //    }
    //    else if (select == 3)
    //    {
    //        _stigmatizeUnit.AddStigma(stigmaList[2]);
    //        GameManager.UI.ClosePopup();
    //        AddStigamScript(stigmaList[2]);
    //    }

    //    //StartCoroutine(QuitScene());
    //    GameManager.Sound.Play("UI/UpgradeSFX/UpgradeSFX");
    //    //OnQuitClick();
    //}

    public void AddStigamScript(Passive stigma)
    {
        UI_Conversation script = GameManager.UI.ShowPopup<UI_Conversation>();
        string scriptKey = "낙인소_" + stigma.Name;
        script.Init(GameManager.Data.ScriptData[scriptKey], false);
        StartCoroutine(QuitScene(script));
    }

    public void OnQuitClick()
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
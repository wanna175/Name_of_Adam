using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StigmaStore : Selectable
{
    private DeckUnit _stigmatizeUnit;

    [SerializeField] private Image _unitImage;

    List<낙인> stigmaList = new();

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

        DeckUnit deckUnit = new();
        stigmaList.Add(deckUnit.GetRandomStigma());

        while (stigmaList.Count < 3)
        {
            낙인 tempStigma = deckUnit.GetRandomStigma();
            if (!stigmaList.Contains(tempStigma) & tempStigma != 낙인.오빠 & tempStigma != 낙인.동생)
            {
                stigmaList.Add(tempStigma);
            }
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

    public void OnStigmaSelect(int select) 
    {
        if (select == 1)
        {
            _stigmatizeUnit.AddStigma(stigmaList[0]);
            GameManager.UI.ClosePopup();
            AddStigamScript(stigmaList[0]);
        }
        else if (select == 2)
        {
            _stigmatizeUnit.AddStigma(stigmaList[1]);
            GameManager.UI.ClosePopup();
            AddStigamScript(stigmaList[1]);
        }
        else if (select == 3)
        {
            _stigmatizeUnit.AddStigma(stigmaList[2]);
            GameManager.UI.ClosePopup();
            AddStigamScript(stigmaList[2]);
        }

        //OnQuitClick();
    }

    public void AddStigamScript(낙인 stigma)
    {
        switch (stigma)
        {
            case 낙인.고양:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_고양"]);
                break;
            case 낙인.강림:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_강림"]);
                break;
            case 낙인.가학:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_가학"]);
                break;
            case 낙인.대죄:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_대죄"]);
                break;
            case 낙인.자애:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_자애"]);
                break;
            case 낙인.처형:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_처형"]);
                break;
            case 낙인.흡수:
                GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["낙인소_흡수"]);
                break;
        }
    }

    public void OnQuitClick()
    {
        StartCoroutine(QuitScene());

        if (GameManager.Data.GameData.isVisitStigma == false)
        {
            GameManager.Data.GameData.isVisitStigma = true;
        }
    }

    private IEnumerator QuitScene()
    {
        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        quitScript.Init(GameManager.Data.ScriptData["낙인소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        SceneChanger.SceneChange("StageSelectScene");
    }
}
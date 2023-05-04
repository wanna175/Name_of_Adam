using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StigmaStore : Selectable
{
    private DeckUnit _stigmatizeUnit;

    [SerializeField] private GameObject button;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Script> scripts = new();
        Script s = new();
        s.name = "≥´¿Œº“";
        s.script = "¿‘¿Â";
        scripts.Add(s);

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }

    public void OnStigmaUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, true, this);
    }

    public override void OnSelect(DeckUnit unit)
    {
        _stigmatizeUnit = unit;
        button.GetComponent<Image>().sprite = unit.Data.Image;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
    }

    public void OnStigmaButtonClick()
    {
        if (_stigmatizeUnit != null) { }
            GameManager.UI.ShowPopup<UI_StigmaSelectButton>().init(this);
    }

    public void OnStigmaSelect(int select) 
    {
        if (select == 1)
        {

        }
        else if (select == 2)
        {
        }
        else if (select == 3)
        {

        }
        else if (select == 4)
        {

        }

        GameManager.UI.ClosePopup();

        OnQuitClick();
    }

    public void OnQuitClick()
    {
        List<Script> scripts = new();
        Script s = new();
        s.name = "≥´¿Œº“";
        s.script = "≈¿Â";
        scripts.Add(s);

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);

        SceneChanger.SceneChange("StageSelectScene");
    }
}
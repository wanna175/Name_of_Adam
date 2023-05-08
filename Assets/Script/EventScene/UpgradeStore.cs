using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStore : Selectable
{
    private DeckUnit _upgradeUnit;

    [SerializeField] private Image _unitImage;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Script> scripts = new List<Script>();

        if(GameManager.Data.GameData.isVisitUpgrade == false)
            scripts = GameManager.Data.ScriptData["강화소_입장_최초"];
        else
            scripts = GameManager.Data.ScriptData["강화소_입장"];

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }

    public void OnUpgradeUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, true, this);
    }

    public override void OnSelect(DeckUnit unit)
    {
        _upgradeUnit = unit;
        _unitImage.sprite = unit.Data.Image;
        _unitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();
    }

    public void OnUpgradeButtonClick()
    {
        if (_upgradeUnit != null)
            GameManager.UI.ShowPopup<UI_UpgradeSelectButton>().init(this);
    }

    public void OnUpgradeSelect(int select) 
    {
        if (select == 1)
        {
            _upgradeUnit.ChangedStat.ATK += 5;
            GameManager.UI.ClosePopup();
            GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["강화소_공격력"]);
        }
        else if (select == 2) 
        {
            _upgradeUnit.ChangedStat.HP += 15;
            GameManager.UI.ClosePopup();
            GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["강화소_체력"]);
        }
        else if (select == 3)
        {
            _upgradeUnit.ChangedStat.SPD += 25;
            GameManager.UI.ClosePopup();
            GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["강화소_속도"]);
        }
        else if (select == 4)
        {
            _upgradeUnit.ChangedStat.ManaCost -= 5;
            GameManager.UI.ClosePopup();
            GameManager.UI.ShowPopup<UI_Conversation>().Init(GameManager.Data.ScriptData["강화소_코스트"]);
        }

        // OnQuitClick();
    }

    public void OnQuitClick()
    {
        StartCoroutine(QuitScene());
        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
        }
    }

    private IEnumerator QuitScene()
    {
        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();

        if (GameManager.Data.GameData.isVisitUpgrade == false)
        {
            GameManager.Data.GameData.isVisitUpgrade = true;
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장_최초"], false);
        }
        else
            quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장"], false);

        yield return StartCoroutine(quitScript.PrintScript());
        SceneChanger.SceneChange("StageSelectScene");
    }
}
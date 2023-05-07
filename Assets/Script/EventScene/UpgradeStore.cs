using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStore : Selectable
{
    private DeckUnit _upgradeUnit;

    [SerializeField] private GameObject button;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Script> scripts = GameManager.Data.ScriptData["강화소_입장"];

        GameManager.UI.ShowPopup<UI_Conversation>().Init(scripts);
    }

    public void OnUpgradeUnitButtonClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, true, this);
    }

    public override void OnSelect(DeckUnit unit)
    {
        _upgradeUnit = unit;
        button.GetComponent<Image>().sprite = unit.Data.Image;

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
        }
        else if (select == 2) 
        {
            _upgradeUnit.ChangedStat.HP += 15;
        }
        else if (select == 3)
        {
            _upgradeUnit.ChangedStat.SPD += 25;
        }
        else if (select == 4)
        {
            _upgradeUnit.ChangedStat.ManaCost -= 5;
        }

        GameManager.UI.ClosePopup();

        OnQuitClick();
    }

    public void OnQuitClick()
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        UI_Conversation quitScript = GameManager.UI.ShowPopup<UI_Conversation>();
        quitScript.Init(GameManager.Data.ScriptData["강화소_퇴장"], false);
        yield return StartCoroutine(quitScript.PrintScript());
        SceneChanger.SceneChange("StageSelectScene");
    }
}
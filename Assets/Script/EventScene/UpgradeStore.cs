using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStore : Selectable
{
    private DeckUnit _upgradeUnit;

    private List<DeckUnit> _playerDeck = new();

    [SerializeField] private GameObject button;

    void Start()
    {
        Debug.Log("UpgradeStore");
        Init();
    }

    private void Init()
    {
        _playerDeck = GameManager.Data.GetDeck();
        //_upgradeHands = GameManager.UI.ShowScene<UI_UpgradeStoreHands>("Event/UI_UpgradeHands");
        //_upgradeHands.SetHands(_playerDeck);

    }

    public void OnUpgradeUnitButtonClick()
    {
        Debug.Log("asd");
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false, true, this);
    }


    public void OnUpgradeButtonClick()
    {
        
    }

    public void OnOutClick()
    {
        SceneChanger.SceneChange("StageSelectScene");
    }

    public override void OnSelect(DeckUnit unit)
    {
        _upgradeUnit = unit;
        button.GetComponent<Image>().sprite = unit.Data.Image;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

    }
}
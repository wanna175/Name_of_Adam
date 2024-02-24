using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_EliteCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _highlight;

    [SerializeField]
    private List<GameObject> _stigmaFrames;

    private List<Image> _stigmaImages;

    [SerializeField]
    private TMP_Text _nameText;

    public Image UnitImage;
    private DeckUnit _deckUnit;

    public void Init(DeckUnit deckUnit)
    {
        _deckUnit = deckUnit;

        _stigmaImages = new List<Image>();
        foreach (var frame in _stigmaFrames)
            _stigmaImages.Add(frame.GetComponentsInChildren<Image>()[1]);

        foreach (var frame in _stigmaFrames)
            frame.SetActive(true);

        UnitImage.sprite = _deckUnit.Data.CorruptImage;
        UnitImage.color = Color.white;
        _nameText.SetText(_deckUnit.Data.Name);

        List<Stigma> stigmas = _deckUnit.GetStigma();
        for (int i = 0; i < _stigmaImages.Count; i++)
        {
            if (i < stigmas.Count)
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetStigma(stigmas[i]);
                _stigmaImages[i].sprite = stigmas[i].Sprite_28;
                _stigmaImages[i].color = Color.white;
            }
            else
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmaImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }

        _highlight.SetActive(false);
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/ClickSFX/UIClick3");
        GameManager.Data.GameData.DeckUnits.Add(_deckUnit);
        GameManager.Data.GameData.FallenUnits.Add(_deckUnit);
        GameManager.OutGameData.SaveData();
        GameManager.SaveManager.SaveGame();

        SceneChanger.SceneChange("StageSelectScene");
    }

    public void OnInfoButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_deckUnit);
        ui.Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
        => _highlight.SetActive(true);

    public void OnPointerExit(PointerEventData eventData)
        => _highlight.SetActive(false);
}

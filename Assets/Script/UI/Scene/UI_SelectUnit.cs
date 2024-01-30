using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectUnit : MonoBehaviour
{
    [SerializeField]
    private Image unitImage;

    public DeckUnit DeckUnit;

    public void Init(DeckUnit deckUnit)
    {
        unitImage.sprite = deckUnit.Data.Image;
        
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
    }
}

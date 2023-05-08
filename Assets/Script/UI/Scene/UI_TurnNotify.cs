using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TurnNotify : UI_Scene
{
    [SerializeField] public GameObject image;

    public void Set(bool playerTurn)
    {
        FadeIn();
        Invoke("FadeOut", 0.5f);

        if (playerTurn)
            image.GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/PlayerTurnText");
        else
            image.GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/UnitTurnText");
    }

    public void FadeIn()
    {
        image.GetComponent<FadeController>().StartFadeIn();
    }

    public void FadeOut()
    {
        image.GetComponent<FadeController>().StartFadeOut();
    }
}
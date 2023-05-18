using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TurnNotify : UI_Scene
{
    public void Set(string turn)
    {
        FadeIn();
        Invoke("FadeOut", 0.5f);

        if (turn == "playerTurn")
            GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/PlayerTurnText");
        else if (turn == "UnitTurn")
            GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/UnitTurnText");
    }

    public void FadeIn()
    {
        GetComponent<FadeController>().StartFadeIn();
    }

    public void FadeOut()
    {
        GetComponent<FadeController>().StartFadeOut();
        Invoke("Destroy", 0.5f);
    }

    private void Destroy()
    {
        GameManager.Resource.Destroy(this.gameObject);
    }
}
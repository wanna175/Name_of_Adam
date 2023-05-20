using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TurnNotify : UI_Scene
{
    const float fadeTime = 0.5f;

    public void Set(string turn)
    {
        FadeIn();
        Invoke("FadeOut", fadeTime);
        //PlayerTurn, UnitTurn µÎ°¡Áö¸¸ ÀÎÀÚ·Î
        
        GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/{turn}Text");
    }

    public void FadeIn()
    {
        GetComponent<FadeController>().StartFadeIn();
    }

    public void FadeOut()
    {
        GetComponent<FadeController>().StartFadeOut();
        Invoke("Destroy", fadeTime);
    }

    private void Destroy()
    {
        GameManager.Resource.Destroy(this.gameObject);
    }
}

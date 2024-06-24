using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ManaGauge : UI_Scene
{
    [SerializeField] Animator ManaAnimator;
    [SerializeField] TextMeshProUGUI _currentMana;
    [SerializeField] UI_CannotEffect cannotEffect;
    public UI_CannotEffect CannotEffect => cannotEffect;

    public void Init()
    {
        cannotEffect.Init(Vector3.one, Vector3.one * 1.2f, 1.5f);
    }

    public void DrawGauge(int max, int current)
    {
        //_bluemanaIMG.fillAmount = (float)current / max;
        SetGauge(current);
        _currentMana.text = current.ToString();
    }

    public void SetGauge(int mana)
    {
        if(mana == 0)
        {
            ManaAnimator.SetTrigger("Mana0");
        }
        else if(0 < mana && mana < 15)
        {
            ManaAnimator.SetTrigger("Mana10");
        }
        else if (15 <= mana && mana < 25)
        {
            ManaAnimator.SetTrigger("Mana20");
        }
        else if (25 <= mana && mana < 35)
        {
            ManaAnimator.SetTrigger("Mana30");
        }
        else if (35 <= mana && mana < 45)
        {
            ManaAnimator.SetTrigger("Mana40");
        }
        else if (45 <= mana && mana < 55)
        {
            ManaAnimator.SetTrigger("Mana50");
        }
        else if (55 <= mana && mana < 65)
        {
            ManaAnimator.SetTrigger("Mana60");
        }
        else if (65 <= mana && mana < 75)
        {
            ManaAnimator.SetTrigger("Mana70");
        }
        else if (75 <= mana && mana < 85)
        {
            ManaAnimator.SetTrigger("Mana80");
        }
        else if (85 <= mana && mana < 100)
        {
            ManaAnimator.SetTrigger("Mana90");
        }
        else if (mana == 100)
        {
            ManaAnimator.SetTrigger("Mana100");
        }
    }
}

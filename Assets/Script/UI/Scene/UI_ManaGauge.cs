using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ManaGauge : UI_Scene
{
    [SerializeField] Animator ManaAnimator;
    [SerializeField] TextMeshProUGUI _currentMana;
    [SerializeField] Image cannotLight;

    private readonly float cannotEffectSpeed = 2.0f;
    
    public void Init()
    {
        cannotLight.gameObject.SetActive(false);
    }

    public void DrawGauge(int max, int current)
    {
        //_bluemanaIMG.fillAmount = (float)current / max;
        SetGauge(current);
        _currentMana.text = current.ToString();
    }

    public void CreateCannotEffect()
    {
        StopAllCoroutines();

        cannotLight.gameObject.SetActive(true);
        StartCoroutine(nameof(SizeUpCannotEffect));
        StartCoroutine(nameof(FadeCannotEffect));
    }

    IEnumerator SizeUpCannotEffect()
    {
        Vector3 initSize = Vector3.one;
        Vector3 goalSize = Vector3.one * 1.2f;
        float time = 0.0f;

        cannotLight.transform.localScale = initSize;

        while (time < 1.0f)
        {
            time += Time.deltaTime * cannotEffectSpeed;
            cannotLight.transform.localScale = Vector3.Lerp(initSize, goalSize, time);
            yield return null;
        }

        cannotLight.transform.localScale = goalSize;
    }

    IEnumerator FadeCannotEffect()
    {
        Color color = cannotLight.color;
        color.a = 1.0f;
        cannotLight.color = color;

        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime * cannotEffectSpeed;
            cannotLight.color = color;
            yield return null;
        }

        color.a = 0.0f;
        cannotLight.color = color;
        cannotLight.gameObject.SetActive(false);
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

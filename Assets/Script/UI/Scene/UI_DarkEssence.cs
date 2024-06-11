using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DarkEssence : UI_Scene
{
    [SerializeField] private TextMeshProUGUI _darkEssence;
    [SerializeField] private Image cannotLight;

    private readonly float cannotEffectSpeed = 2.0f;

    public void Init()
    {
        cannotLight.gameObject.SetActive(false);

        Refresh();
    }

    public void Refresh() 
    {
        _darkEssence.text = GameManager.Data.GameData.DarkEssence.ToString();
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
}

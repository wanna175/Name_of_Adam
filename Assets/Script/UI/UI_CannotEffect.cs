using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CannotEffect : MonoBehaviour
{
    [SerializeField] private Image cannotLight;
    private Vector3 initSize;
    private Vector3 goalSize;
    private float effectSpeed;

    public void Init(Vector3 initSize, Vector3 goalSize, float effectSpeed)
    {
        this.initSize = initSize;
        this.goalSize = goalSize;
        this.effectSpeed = effectSpeed;

        cannotLight.gameObject.SetActive(false);
    }

    public void Create()
    {
        StopAllCoroutines();

        cannotLight.gameObject.SetActive(true);
        StartCoroutine(nameof(SizeUpCannotEffect));
        StartCoroutine(nameof(FadeCannotEffect));
    }

    IEnumerator SizeUpCannotEffect()
    {
        float time = 0.0f;

        cannotLight.transform.localScale = initSize;

        while (time < 1.0f)
        {
            time += Time.deltaTime * effectSpeed;
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
            color.a -= Time.deltaTime * effectSpeed;
            cannotLight.color = color;
            yield return null;
        }

        color.a = 0.0f;
        cannotLight.color = color;
        cannotLight.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNodeBackLight : MonoBehaviour
{
    private SpriteRenderer renderer;
    IEnumerator coro;
    float fadeTime = 0.3f;
    float curTime = 0;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        coro = null;
    }

    public void FadeIn()
    {
        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }

        Color c = renderer.color;
        coro = FadeInCoro(c);
        StartCoroutine(coro);
    }
    IEnumerator FadeInCoro(Color c)
    {
        while (fadeTime > curTime)
        {
            c.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            renderer.color = c;
            curTime += Time.deltaTime;

            yield return null;
        }
        c.a = 1;
        renderer.color = c;
    }

    public void FadeOut()
    {

        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }

        Color c = renderer.color;
        coro = FadeOutCoro(c);
        StartCoroutine(coro);
    }
    IEnumerator FadeOutCoro(Color c)
    {
        while (curTime > 0)
        {
            c.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            renderer.color = c;
            curTime -= Time.deltaTime;

            yield return null;
        }
        c.a = 0;
        renderer.color = c;
    }
}

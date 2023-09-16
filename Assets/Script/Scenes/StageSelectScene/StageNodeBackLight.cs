using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNodeBackLight : MonoBehaviour
{
    private SpriteRenderer _renderer;
    IEnumerator coro;
    const float fadeTime = 0.3f;
    float curTime = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        coro = null;
    }

    public void SetSprite(string name) => _renderer.sprite = GameManager.Resource.Load<Sprite>($"Arts/StageSelect/BackLight/{name}");

    public void SetVisible() => _renderer.color = new Color(1, 1, 1, 1);

    public void FadeIn()
    {
        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }

        Color c = _renderer.color;
        coro = FadeInCoro(c);
        StartCoroutine(coro);
    }
    IEnumerator FadeInCoro(Color c)
    {
        while (fadeTime > curTime)
        {
            c.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            _renderer.color = c;
            curTime += Time.deltaTime;

            yield return null;
        }
        c.a = 1;
        _renderer.color = c;
    }

    public void FadeOut()
    {

        if (coro != null)
        {
            StopCoroutine(coro);
            coro = null;
        }

        Color c = _renderer.color;
        coro = FadeOutCoro(c);
        StartCoroutine(coro);
    }

    IEnumerator FadeOutCoro(Color c)
    {
        while (curTime > 0)
        {
            c.a = Mathf.Lerp(0, 1, curTime / fadeTime);
            _renderer.color = c;
            curTime -= Time.deltaTime;

            yield return null;
        }
        c.a = 0;
        _renderer.color = c;
    }
}

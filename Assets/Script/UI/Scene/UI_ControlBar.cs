using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlBar : UI_Scene
{
    [SerializeField] public UI_Hands UI_Hands;
    [SerializeField] public UI_PlayerHP UI_PlayerHP;
    [SerializeField] public UI_PlayerSkill UI_PlayerSkill;
    [SerializeField] public UI_DarkEssence UI_DarkEssence;
    [SerializeField] public UI_ManaGauge UI_ManaGauge;
    [SerializeField] public Animator UI_Aniamtor;
    [SerializeField] private GameObject _active;

    public void EndPlayerHit()
        => UI_Aniamtor.SetBool("isPlayerHit", false);

    public void ControlBarActive()
    {
        _active.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color c = _active.GetComponent<Image>().color;

        while (c.a < 1)
        {
            c.a += 0.01f;

            _active.GetComponent<Image>().color = c;

            yield return null;
        }
    }

    public void ControlBarInactive()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color c = _active.GetComponent<Image>().color;

        while (c.a > 0)
        {
            c.a -= 0.01f;

            _active.GetComponent<Image>().color = c;

            yield return null;
        }

        _active.SetActive(false);
    }
}

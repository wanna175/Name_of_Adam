using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] LogoImages;

    private void Start()
    {
        foreach(SpriteRenderer sr in LogoImages)
            sr.color = Color.clear;

        StartCoroutine(ViewLogo());
    }

    private IEnumerator ViewLogo()
    {
        foreach (SpriteRenderer sr in LogoImages)
        {
            sr.color = Color.black;

            float time = 0;
            float FadeTime = 1;

            while (time <= FadeTime)
            {
                time += Time.deltaTime;
                float light = (time / FadeTime);
                light = (light > 1) ? 1 : light;

                sr.color = new Color(light, light, light, 1);

                yield return null;
            }
            
            yield return new WaitForSeconds(1);

            time = FadeTime;

            while (time > 0)
            {
                time -= Time.deltaTime;
                float light = (time / FadeTime);
                light = (light < 0) ? 0 : light;

                sr.color = new Color(light, light, light, 1);
                
                yield return null;
            }

            sr.color = Color.clear;
        }

        EndScene();
    }

    private void EndScene()
    {
        SceneChanger.SceneChange("MainScene");
    }
}

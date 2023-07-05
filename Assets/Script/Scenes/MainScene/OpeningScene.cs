using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    //[SerializeField] SpriteRenderer[] LogoImages;
    [SerializeField] Image image; // 검은 화면
    [SerializeField] GameObject button; // 클릭 버튼

    

    public void FadeButton()
    {
        Debug.Log("버튼클릭");
        button.SetActive(false);
        StartCoroutine(FadeCoroutine());

        
    }

    private IEnumerator FadeCoroutine()
    {
        float fadeCount = 0;
        while(fadeCount >= 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, fadeCount);
        }
        
        
    }

}

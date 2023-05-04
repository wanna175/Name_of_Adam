using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ManaGauge : UI_Scene
{
    enum Objects
    {
        Fill,
        ManaCostText,
    }

    private void Awake()
    {
        Bind<GameObject>(typeof(Objects));
    }
    
    public void DrawGauge(int max, int current)
    {
        GetObject((int)Objects.Fill).GetComponent<Image>().fillAmount = (float)current / max;
        GetObject((int)Objects.ManaCostText).GetComponent<TextMeshProUGUI>().text = current.ToString();
    }
}

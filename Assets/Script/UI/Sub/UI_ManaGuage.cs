using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManaGuage : UI_Scene
{
    enum Objects
    {
        ManaGuageSlider,
        ManaCostText,
    }

    private void Awake()
    {
        Bind<GameObject>(typeof(Objects));
        //GameManager.Battle.Data.SetManaGuage(this);
    }

    public void DrawGauge(int currentMana)
    {
        GetObject((int)Objects.ManaGuageSlider).GetComponent<Slider>().value = currentMana;
        GetObject((int)Objects.ManaCostText).GetComponent<Text>().text = currentMana.ToString();
    }
}

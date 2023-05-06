using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UI_Option : UI_Popup
{
    [SerializeField] AudioMixer MasterMixer;

    int ScreenX = 1920;
    int ScreenY = 1080;
    bool isWindow = false;

    public void ChangeResolution(TextMeshProUGUI text)
    {
        int ScreenX = Int32.Parse(text.text.Split(" x ")[0]);
        int ScreenY = Int32.Parse(text.text.Split(" x ")[1]);

        Debug.Log(ScreenX + ", " + ScreenY);

        SetScreen();
    }

    public void ChangeWindow(Toggle toggle)
    {
        isWindow = toggle.isOn;

        SetScreen();
    }

    public void UpdateVolume(GameObject go)
    {
        string text = go.transform.GetChild(0).GetComponent<Text>().text;
        float slider = go.transform.GetChild(1).GetComponent<Slider>().value;
        slider = (slider == -40) ? -80 : slider;

        MasterMixer.SetFloat(text, slider);
    }
    
    private void SetScreen() => Screen.SetResolution(ScreenX, ScreenY, isWindow);


    public void QuitOption() => GameManager.UI.ClosePopup();
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UI_Option : UI_Popup
{
    [SerializeField] AudioMixer MasterMixer;
    [SerializeField] List<GameObject> SoundSliders;
    [SerializeField] TextMeshProUGUI ResolutionText;
    [SerializeField] Toggle WindowToggle;

    int ScreenX = 1920;
    int ScreenY = 1080;
    bool isWindow = false;

    private void OnEnable()
    {
        SetSlider();
        SetResolution();
    }

    private void SetSlider()
    {
        foreach (GameObject slider in SoundSliders)
        {
            string text = slider.transform.GetChild(0).GetComponent<Text>().text;
            float value;

            MasterMixer.GetFloat(text, out value);
            slider.transform.GetChild(1).GetComponent<Slider>().value = value;
        }
    }

    private void SetResolution()
    {
        ScreenX = Screen.currentResolution.width;
        ScreenY = Screen.currentResolution.height;
        bool window = !Screen.fullScreen;

        ResolutionText.text = ScreenX + " x " + ScreenY;
        WindowToggle.isOn = window;
    }


    public void ChangeResolution(TextMeshProUGUI text)
    {
        ScreenX = Int32.Parse(text.text.Split(" x ")[0]);
        ScreenY = Int32.Parse(text.text.Split(" x ")[1]);

        SetScreen();
    }

    public void ChangeWindow(Toggle toggle)
    {
        GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        isWindow = toggle.isOn;

        SetScreen();
    }

    public void UpdateVolume(GameObject go)
    {
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        string text = go.transform.GetChild(0).GetComponent<Text>().text;
        float slider = go.transform.GetChild(1).GetComponent<Slider>().value;
        slider = (slider == -40) ? -80 : slider;

        MasterMixer.SetFloat(text, slider);
    }
    
    private void SetScreen() => Screen.SetResolution(ScreenX, ScreenY, !isWindow);

    public void QuitOption() => GameManager.UI.ClosePopup();

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        Application.Quit();
    }
}

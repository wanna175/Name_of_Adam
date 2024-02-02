using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UI_Option : UI_Popup
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Toggle WindowToggle;

    private List<Resolution> resolutions;

    private Resolution currentResolution;
    private bool isWindowed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
            QuitOption();
        }
    }


    private void OnEnable()
    {
        InitUI();
    }

    private void InitUI()
    {
        // 저장된 데이터 불러오기
        currentResolution = new Resolution();
        currentResolution.width = GameManager.OutGameData.GetResolutionWidth();
        currentResolution.height = GameManager.OutGameData.GetResolutionHeight();
        isWindowed = GameManager.OutGameData.IsWindowed();

        // UI 세팅
        resolutions = new List<Resolution>();
        resolutions.Add(GetResolution(1920, 1080, 144));
        resolutions.Add(GetResolution(1280, 720, 144));
        resolutions.Add(GetResolution(640, 480, 144));

        resolutionDropdown.onValueChanged.AddListener(ResolutionDropdownChanged);
        resolutionDropdown.options.Clear();

        foreach (var resolution in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{resolution.width} x {resolution.height}";
            resolutionDropdown.options.Add(option);
        }

        resolutionDropdown.RefreshShownValue();

        WindowToggle.onValueChanged.AddListener(WindowToggleChanged);
        WindowToggle.isOn = isWindowed;
    }

    private Resolution GetResolution(int width, int height, int refreshRate)
    {
        Resolution resolution = new Resolution();
        resolution.width = width;
        resolution.height = height;
        resolution.refreshRate = refreshRate;
        return resolution;
    }

    public void ResolutionDropdownChanged(int idx)
    {
        GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        currentResolution = resolutions[idx];

        GameManager.OutGameData.SetResolution(currentResolution);
    }

    public void WindowToggleChanged(bool isOn)
    {
        GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        isWindowed = isOn;

        GameManager.OutGameData.SetWindow(isWindowed);
    }

    public void UpdateVolume(GameObject go)
    {
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        string text = go.transform.GetChild(0).GetComponent<Text>().text;
        float slider = go.transform.GetChild(1).GetComponent<Slider>().value;
        slider = (slider == -40) ? -80 : slider;
    }

    public void QuitOption()
    {
        GameManager.UI.ClosePopup(this);
        GameManager.UI.IsCanESC = true;

        // 저장 추가
    }
}

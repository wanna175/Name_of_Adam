using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Option : UI_Popup
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Toggle WindowToggle;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;

    [SerializeField] private GameObject UI_Credit;

    private List<Resolution> resolutions;
    private int currentLanguage;
    private int currentResolution;
    private bool isWindowed = false;
    private float masterPower;
    private float BGMPower;
    private float SEPower;

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
        resolutions = GameManager.OutGameData.GetAllResolution();
        currentLanguage = GameManager.OutGameData.GetLanguage();
        currentResolution = GameManager.OutGameData.GetResolutionIndex();
        isWindowed = GameManager.OutGameData.IsWindowed();

        masterPower = GameManager.OutGameData.GetMasterSoundPower();
        BGMPower = GameManager.OutGameData.GetBGMSoundPower();
        SEPower = GameManager.OutGameData.GetSESoundPower();

        // UI 세팅
        languageDropdown.onValueChanged.AddListener(GameManager.Locale.LanguageChanged);
        resolutionDropdown.onValueChanged.AddListener(ResolutionDropdownChanged);
        resolutionDropdown.options.Clear();

        foreach (var resolution in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{resolution.width} x {resolution.height}";
            resolutionDropdown.options.Add(option);
        }

        languageDropdown.value = currentLanguage;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.value = currentResolution;

        WindowToggle.onValueChanged.AddListener(WindowToggleChanged);
        WindowToggle.isOn = isWindowed;

        masterSlider.onValueChanged.AddListener(MasterSliderChanged);
        BGMSlider.onValueChanged.AddListener(BGMSliderChanged);
        SESlider.onValueChanged.AddListener(SESliderChanged);

        masterSlider.value = masterPower;
        BGMSlider.value = BGMPower;
        SESlider.value = SEPower;
    }

    public void LanguageDropdownChanged()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
    }

    private void ResolutionDropdownChanged(int idx)
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.OutGameData.SetResolution(idx);
    }

    private void WindowToggleChanged(bool isOn)
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        isWindowed = isOn;
        GameManager.OutGameData.SetWindow(isWindowed);
    }

    private void MasterSliderChanged(float power)
    {
        masterPower = power;
        GameManager.OutGameData.SetMasterSoundPower(power);
        GameManager.Sound.SetSoundVolume(Sounds.BGM);
        GameManager.Sound.SetSoundVolume(Sounds.Effect);
    }

    private void BGMSliderChanged(float power)
    {
        BGMPower = power;
        GameManager.OutGameData.SetBGMSoundPower(power);
        GameManager.Sound.SetSoundVolume(Sounds.BGM);
    }

    private void SESliderChanged(float power)
    {
        SEPower = power;
        GameManager.OutGameData.SetSESoundPower(power);
        GameManager.Sound.SetSoundVolume(Sounds.Effect);
    }

    public void ReSetOption()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.OutGameData.ReSetOption();
        InitUI();
    }

    public void CreditOption()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_Credit.SetActive(true);
    }

    public void QuitOption()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.ClosePopup(this);
        GameManager.UI.IsCanESC = true;

        GameManager.OutGameData.SaveData();
    }
}

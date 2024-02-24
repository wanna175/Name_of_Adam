using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class LocaleManager : MonoBehaviour
{
    Locale currentLocale;
    bool isChangingLanguage;

    int currentLocaleIndex;
    /// <summary> 0 = EN, 1 = KR </summary>
    public int CurrentLocaleIndex => currentLocaleIndex;

    public void Init()
    {
        isChangingLanguage = false;

        LanguageChanged(GameManager.OutGameData.GetLanguage());
    }

    public string GetLocalizedPlayerSkillInfo(int skillIdx)
    {
        switch (skillIdx)
        {
            case 1: return GetLocalizedPlayerSkillInfo("Lowers the faith of a designated enemy by 1.");
            case 2:
                if (GameManager.OutGameData.IsUnlockedItem(54))
                    return GetLocalizedPlayerSkillInfo("Deals 20 damage in a massive cross range near the designated enemy. It reduces the enemy's faith by 1 upon attacking.");
                else
                    return GetLocalizedPlayerSkillInfo("Deals 20 damage in a massive cross range near the designated enemy.");
            case 3: return GetLocalizedPlayerSkillInfo("Moves an ally one space and grants a speed boost.");
            case 4:
                if (GameManager.OutGameData.IsUnlockedItem(62))
                    return GetLocalizedPlayerSkillInfo("Heals the designated ally or enemy by 20 health.");
                else
                    return GetLocalizedPlayerSkillInfo("Heals the designated ally or enemy by 15 health.");
            case 5: return GetLocalizedPlayerSkillInfo("Brings back a designated ally.");
            case 6:
                if (GameManager.OutGameData.IsUnlockedItem(64))
                    return GetLocalizedPlayerSkillInfo("Bestows a curse on the designated enemy.");
                else
                    return GetLocalizedPlayerSkillInfo("Bestows a curse and increases attack on the designated enemy.");
            case 7: return GetLocalizedPlayerSkillInfo("Deals 30 damage to the designated unit.");
            case 8: return GetLocalizedPlayerSkillInfo("Bestows malevolence 3 times and reduces faith by 1 to the designated ally.");
            case 9: return GetLocalizedPlayerSkillInfo("Summons the apostle onto the battlefield.");
        }

        Debug.Log($"PlayerSkill Info Localization is faied.");
        return "";
    }

    public string GetLocalizedUpgrade(string upgradeInfo) => GetLocalizedString("UpgradeTable", upgradeInfo);

    public string GetLocalizedEventScene(string eventSceneStr) => GetLocalizedString("EventSceneTable", eventSceneStr);

    public string GetLocalizedPlayerSkillInfo(string playerSkillInfo) => GetLocalizedString("PlayerSkillInfoTable", playerSkillInfo);

    public string GetLocalizedPlayerSkillName(string playerSkillName) => GetLocalizedString("PlayerSkillNameTable", playerSkillName);

    public string GetLocalizedScriptName(string scriptName) => GetLocalizedString("ScriptNameTable", scriptName);

    public string GetLocalizedScriptInfo(string scriptInfo) => GetLocalizedString("ScriptInfoTable", scriptInfo);

    public string GetLocalizedBuffName(string buffName) => GetLocalizedString("BuffNameTable", buffName);

    public string GetLocalizedBuffInfo(string buffInfo) => GetLocalizedString("BuffInfoTable", buffInfo);

    public string GetLocalizedStigmaName(string stigmaName) => GetLocalizedString("StigmaNameTable", stigmaName);

    public string GetLocalizedStigmaInfo(string stigmaInfo) => GetLocalizedString("StigmaInfoTable", stigmaInfo);

    public string GetLocalizedUnitName(string unitName) => GetLocalizedString("UnitTable", unitName);

    private string GetLocalizedString(string tableName, string key)
    {
        string str = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, key, currentLocale);
        if (str.Contains("No translation"))
        {
            Debug.Log($"'{key}' Localization is faied.");
            return key;
        }

        return str;
    }

    public void LanguageChanged(int localeIndex)
    {
        if (isChangingLanguage)
            return;

        StartCoroutine(ChangeLanuage(localeIndex));
    }

    IEnumerator ChangeLanuage(int localeIndex)
    {
        isChangingLanguage = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        currentLocale = LocalizationSettings.SelectedLocale;
        currentLocaleIndex = localeIndex;
        GameManager.OutGameData.SetLanguage(localeIndex);

        Debug.Log(LocalizationSettings.SelectedLocale.LocaleName);

        isChangingLanguage = false;
    }
}

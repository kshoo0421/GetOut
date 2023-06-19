using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OptionManager : BehaviorSingleton<OptionManager>
{
    #region Field
    /* Change Locale */
    bool isChanging;

    #endregion

    #region Monobehaviour
    void Awake()
    {
        // Set Language
        ChangeLocale(PlayerPrefs.GetInt("LocaleNum")); 
    }
    #endregion

    #region Change Locale
    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        PlayerPrefs.SetInt("LocaleNum", index);
        PlayerPrefs.Save();
        StartCoroutine(ChangeRoutine(index));
    }

    IEnumerator ChangeRoutine(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
    #endregion
}
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : BehaviorSingleton<LanguageManager>
{
    #region Field
    /* Change Locale */
    bool isChanging;
    #endregion

    #region Change Locale
    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

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
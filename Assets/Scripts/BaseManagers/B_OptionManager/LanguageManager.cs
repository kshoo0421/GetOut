using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ ±¸Çö
    private static LanguageManager instance;
    private LanguageManager() { }

    public static LanguageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LanguageManager();
            }
            return instance;
        }
    }
    #endregion

    bool isChanging;
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
}

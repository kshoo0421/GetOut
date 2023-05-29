using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    #region Field
    /* Singleton */
    static LanguageManager instance;
    /* Change Locale */
    bool isChanging;
    #endregion

    #region Singleton
    LanguageManager() { }

    public static LanguageManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<LanguageManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }
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
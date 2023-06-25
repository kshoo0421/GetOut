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
        ChangeLocale(PrefsBundle.LocaleNum); 
    }
    #endregion

    #region Change Locale
    public void ChangeLocale(int index)
    {
        if (isChanging)
            return;

        PrefsBundle.Instance.SetInt(IntPrefs.LocaleNum, index);
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

    #region Change Resolution
    public void SetResolution(int index)
    {
        float height, width;
        switch (index)
        {
            case 0:
                height = 9f;
                width = 16f;
                break;
            case 1:
                height = 3f;
                width = 4f;
                break;
            default:
                height = 9f;
                width = 16f;
                break;
        }
        CameraResolution.Instance.SetCameraResolution(height, width);
    }
    #endregion
}
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

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
        Debug.Log(PlayerPrefs.GetInt("LocalNum"));
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

    #region Change Scene
    public void ChangetoScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
}
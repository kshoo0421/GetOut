using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OptionManager : BehaviorSingleton<OptionManager>
{
    #region Field
    /* Change Locale */
    private bool isChanging;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        InitializeVolume();
        // Set Language
        ChangeLocale(PrefsBundle.LocaleNum);
    }
    #endregion 

    #region Option - Sound
    private void InitializeVolume() // Initialize
    {
        if (PrefsBundle.isVolumeFirst == 0)
        {
            SoundEffectManager.volume = 1.0f;
            BgmManager.bgm.volume = 1.0f;
            PrefsBundle.Instance.SetInt(IntPrefs.isVolumeFirst, 1);
        }
        else
        {
            SoundEffectManager.isMute = (PrefsBundle.isSEMute == 1);
            BgmManager.bgm.mute = (PrefsBundle.isBGMMute == 1);

            SoundEffectManager.volume = PrefsBundle.SEVolume;
            BgmManager.bgm.volume = PrefsBundle.BGMVolume;
        }
    }

    public void SetSEVolume(float value)    // for slider
    {
        if (value == 0) return; // prevent initialization when destroy
        SoundEffectManager.volume = value;
        PrefsBundle.Instance.SetFloat(FloatPrefs.SEVolume, SoundEffectManager.volume);
    }

    public void MakeSEMute(bool b)
    {
        PrefsBundle.Instance.SetInt(IntPrefs.isSEMute, b ? 1 : 0);
        SoundEffectManager.isMute = b;
    }

    public void SetBGMVolume(float value)   // for slider
    {
        if (value == 0) return; // prevent initialization when destroy
        BgmManager.bgm.volume = value;
        PrefsBundle.Instance.SetFloat(FloatPrefs.BGMVolume, BgmManager.bgm.volume);
    }

    public void MakeBGMMute(bool b)
    {
        PrefsBundle.Instance.SetInt(IntPrefs.isBGMMute, b ? 1 : 0);
        BgmManager.bgm.mute = b;
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

    private IEnumerator ChangeRoutine(int index)
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
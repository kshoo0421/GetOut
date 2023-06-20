using UnityEngine;

public enum IntPrefs { isBannerOpen, LocaleNum, isVolumeFirst, isSEMute, isBGMMute };
public enum FloatPrefs { SEVolume, BGMVolume };
public enum StringPrefs { };

public class PrefsBundle : BehaviorSingleton<PrefsBundle>
{
    #region Field
    // int
    public static int isBannerOpen;
    public static int LocaleNum;
    public static int isVolumeFirst;
    public static int isSEMute;
    public static int isBGMMute;

    // float
    public static float SEVolume;
    public static float BGMVolume;
    // string

    // PrefsData
    public static PrefsData prefsData;
    #endregion

    #region Monobehaviour
    void Awake()
    {
        UpdatePrefs();
        prefsData = new PrefsData();
    }

    void UpdatePrefs()
    {
        UpdateInt();
        UpdateFloat();
        UpdateString();
    }
    #endregion

    #region int
    public void SetInt(IntPrefs prefs, int value)
    {
        string key = "none";
        switch(prefs)
        {
            case IntPrefs.isBannerOpen:
                key = "isBannerOpen";
                break;

            case IntPrefs.LocaleNum:
                key = "LocaleNum";
                break;

            case IntPrefs.isVolumeFirst:
                key = "isVolumeFirst";
                break;

            case IntPrefs.isSEMute: 
                key = "isSEMute"; 
                break;

            case IntPrefs.isBGMMute:
                key = "isBGMMute";
                break;

            default:
                key = "none"; 
                break;
        }
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
        UpdateInt();
    }

    void UpdateInt()
    {
        isBannerOpen = PlayerPrefs.GetInt("isBannerOpen");
        LocaleNum = PlayerPrefs.GetInt("LocaleNum");
        isVolumeFirst = PlayerPrefs.GetInt("isVolumeFirst");
        isSEMute = PlayerPrefs.GetInt("isSEMute");
        isBGMMute = PlayerPrefs.GetInt("isBGMMute");
    }
    #endregion

    #region float
    public void SetFloat(FloatPrefs prefs, float value)
    {
        string key = "none";
        switch (prefs)
        {
            case FloatPrefs.SEVolume:
                key = "SEVolume";
                break;

            case FloatPrefs.BGMVolume:
                key = "BGMVolume";
                break;

            default:
                key = "none";
                break;
        }
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
        UpdateFloat();
    }

    void UpdateFloat()
    {
        SEVolume = PlayerPrefs.GetFloat("SEVolume");
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
    }
    #endregion

    #region string
    public void SetString(StringPrefs prefs, float value)
    {
        string key = "none";
        switch (prefs)
        {
            default:
                key = "none";
                break;
        }
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
        UpdateString();
    }

    void UpdateString()
    {
    }
    #endregion

    #region PrefsData
    public void GetPrefsData()
    {
        prefsData.isBannerOpen = isBannerOpen;
        prefsData.LocaleNum = LocaleNum;
        prefsData.isVolumeFirst = isVolumeFirst;
        prefsData.isSEMute = isSEMute;
        prefsData.isBGMMute = isBGMMute;
        prefsData.SEVolume = SEVolume.ToString();
        prefsData.BGMVolume = BGMVolume.ToString();
    }

    public void SetPrefsData(PrefsData pref)
    {
        isBannerOpen = (int)pref.isBannerOpen;
        LocaleNum = (int)pref.LocaleNum;
        isVolumeFirst = (int)pref.isVolumeFirst;
        isSEMute = (int)pref.isSEMute;
        isBGMMute = (int)pref.isBGMMute;
        SEVolume = float.Parse(pref.SEVolume);
        BGMVolume = float.Parse(pref.BGMVolume);
        UpdatePrefs();
    }
    #endregion
}

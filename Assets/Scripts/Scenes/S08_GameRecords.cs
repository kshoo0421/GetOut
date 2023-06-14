using UnityEngine;

public class S08_GameRecords : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers(); 
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}
using UnityEngine;

public class S07_RecordRoom : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}
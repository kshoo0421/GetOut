using UnityEngine;

public class S13_Friends : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}

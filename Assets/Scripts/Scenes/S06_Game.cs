using UnityEngine;

public class S06_Game : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}
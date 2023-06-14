using UnityEngine;

public class S02_WaitingRoom : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}
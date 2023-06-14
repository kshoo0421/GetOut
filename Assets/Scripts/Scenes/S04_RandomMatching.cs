using UnityEngine;

public class S04_RandomMatching : Scenes
{
    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}

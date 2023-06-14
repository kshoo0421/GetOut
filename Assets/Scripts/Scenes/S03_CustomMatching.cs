using UnityEngine;

public class S03_CustomMatching : Scenes
{
    #region monobehaviour
    void Start()
    { 
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion
}

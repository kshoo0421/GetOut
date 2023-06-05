using UnityEngine;

public class S10_Guide : MonoBehaviour
{
    #region Field
    /* Managers */
    OptionManager optionManager;
    GoogleAdMobManager googleAdMobManager;
    #endregion

    #region monobehaviour
    void Start()
    {
        SetManagers();
        googleAdMobManager.ToggleBannerAd();
    }
    #endregion

    #region Set Managers
    void SetManagers()
    {
        optionManager = OptionManager.Instance;
        googleAdMobManager = GoogleAdMobManager.Instance;
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => optionManager.ChangetoScene(sceneIndex);
    #endregion

}
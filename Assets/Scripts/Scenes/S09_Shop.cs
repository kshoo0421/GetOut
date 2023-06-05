using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S09_Shop : MonoBehaviour
{
    #region Field
    /* Managers */
    OptionManager optionManager;
    GoogleAdMobManager googleAdMobManager;
    E_GooglePayManager googlePayManager;

    /* Ads */
    [SerializeField] Button[] AdsBtns;  // 0 : 배너 광고, 
    [SerializeField] Button RewardAdsBtn;

    /* CurState */
    [SerializeField] TMP_Text curStateTmp;

    #endregion

    #region monobehaviour
    void Start()
    {
        SetManagers();
        SetAd();
    }

    void Update()
    {
        CanShowAd();
        googleAdMobManager.ListenToAdEvents();
    }
    #endregion

    #region Set Managers
    void SetManagers()
    {
        optionManager = OptionManager.Instance;

        googleAdMobManager = GoogleAdMobManager.Instance;
        googlePayManager = E_GooglePayManager.Instance;
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => optionManager.ChangetoScene(sceneIndex);
    #endregion

    #region Ads
    void SetAd()
    {
        googleAdMobManager.ToggleBannerAd();
        googleAdMobManager.LoadRewardedAd();
    }
    void CanShowAd() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // 리워드 광고를 틀 수 있으면 활성화

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // 배너 광고 설정/해제

    public void ShowRewardedAd() => googleAdMobManager.ShowRewardedAd();

    #endregion
}
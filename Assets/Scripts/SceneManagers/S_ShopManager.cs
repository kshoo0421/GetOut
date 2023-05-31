using UnityEngine;
using UnityEngine.UI;

public class S_ShopManager : MonoBehaviour
{
    #region Field
    /* Managers */
    B_TotalGameManager totalGameManager;
    GoogleAdMobManager googleAdMobManager;
    B_SceneChangeManager sceneChanger;

    /* Ads */
    [SerializeField] Button RewardAdsBtn;
    #endregion

    #region monobehaviour
    private void Start()
    {
        SetManagers();
    }

    private void Update()
    {
        CheckRewardAdsBtn();
    }
    #endregion

    #region Set Managers
    void SetManagers()
    {
        totalGameManager = B_TotalGameManager.Instance;
        googleAdMobManager = totalGameManager.googleAdMobManager;
        sceneChanger = totalGameManager.b_SceneChangeManager;
    }
    #endregion

    #region Change Scene
    public void ChangeToScene(int sceneIndex) => sceneChanger.ChangetoScene(sceneIndex);
    #endregion

    #region Ads
    void CheckRewardAdsBtn() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // 리워드 광고를 틀 수 있으면 활성화

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // 배너 광고 설정/해제

    public void ShowRewardedAd()
    {
        googleAdMobManager.LoadRewardedAd();
        googleAdMobManager.ShowRewardedAd();
    }
    #endregion
}
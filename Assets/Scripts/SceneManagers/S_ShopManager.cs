using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_ShopManager : MonoBehaviour
{
    #region monobehaviour
    private TotalGameManager totalGameManager;
    private GoogleAdMobManager googleAdMobManager;
    [SerializeField] private Button RewardAdsBtn;

    private void Awake()
    {
        totalGameManager = TotalGameManager.Instance;
    }

    private void Start()
    {
        googleAdMobManager = totalGameManager.googleAdMobManager;
    }

    private void Update()
    {
        RewardAdsBtn.interactable = googleAdMobManager.CanShowAd();
    }
    #endregion

    #region ¾À º¯°æ
    private B_SceneChangeManager sceneChanger = B_SceneChangeManager.Instance;

    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion

    #region ±¤°í ¼³Á¤
    public void ToggleBannerAd(bool b)
    {
        Debug.Log(b);
        googleAdMobManager.ToggleBannerAd(b);
    }

    public void ShowRewardedAd()
    {
        googleAdMobManager.LoadRewardedAd();
        googleAdMobManager.ShowRewardedAd();
    }
    #endregion
}

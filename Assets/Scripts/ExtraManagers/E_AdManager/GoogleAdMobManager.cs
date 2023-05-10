using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds.Api;

public class GoogleAdMobManager : MonoBehaviour
{
    //public bool isTestMode;
    //public TextMeshPro LogText;
    //public Button FrontAdsBtn, RewardAdsBtn;

    //void Start()
    //{
    //    LoadBannerAd();
    //    LoadFrontAd();
    //    LoadRewardAd();
    //}

    //void Update()
    //{
    //    FrontAdsBtn.interactable = frontAd.IsLoaded();
    //    RewardAdsBtn.interactable = rewardAd.IsLoaded();
    //}

    //AdRequest GetAdRequest()
    //{
    //    return new AdRequest.Builder().AddTestDevice("B74D8931364799EC").Build();
    //}

    //#region ¹è³Ê ±¤°í
    //const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    //const string bannerID = "ca-app-pub-5086433509319711/6620949679";
    //BannerView bannerAd;

    //void LoadBannerAd()
    //{
    //    bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID, 
    //        AdSize.SmartBanner, AdPosition.Bottom);
    //    bannerAd.LoadAd(GetAdRequest());
    //    ToggleBannerAd(false);
    //}

    //public void ToggleBannerAd(bool b)
    //{
    //    if (b) bannerAd.Show();
    //    else bannerAd.Hide();
    //}
    //#endregion

    //#region Àü¸é ±¤°í
    //const string frontTestID = "ca-app-pub-3940256099942544/1033173712";
    //const string frontID = "ca-app-pub-5086433509319711/6429377983";
    //InterstitialAd frontAd;

    //void LoadFrontAd()
    //{
    //    frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
    //    frontAd.LoadAd(GetAdRequest());
    //    frontAd.OnAdClosed += (sender, e) =>
    //    {
    //        LogText.text = "Àü¸é±¤°í ¼º°ø";
    //    };
    //}

    //public void ShowFrontAd()
    //{
    //    frontAd.Show();
    //    LoadFrontAd();
    //}
    //#endregion

    //#region ¸®¿öµå ±¤°í
    //const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    //const string rewardID = "ca-app-pub-5086433509319711/4983638216";
    //RewardedAd rewardAd;

    //void LoadRewardAd()
    //{
    //    this.rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
    //    this.rewardAd.LoadAd(GetAdRequest());
    //    this.rewardAd.OnUserEarnedReward += (sender, e) =>
    //    {
    //        LogText.text = "¸®¿öµå ±¤°í ¼º°ø";
    //    };
    //}

    //public void ShowRewardAd()
    //{
    //    rewardAd.Show();
    //    LoadRewardAd();
    //}
    //#endregion
}

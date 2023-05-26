using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class GoogleAdMobManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static GoogleAdMobManager instance;
    private GoogleAdMobManager() { }
    public static GoogleAdMobManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<GoogleAdMobManager>();
        }
    }
    #endregion

    public bool isTestMode;
    public TMP_Text LogText;
    public Button FrontAdsBtn, RewardAdsBtn;

    void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    #region ¸®¿öµå ±¤°í
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";    // Å×½ºÆ®¿ë
    //private string _adUnitId = "ca-app-pub-5086433509319711/6109871255";  // Âð
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";    // Å×½ºÆ®¿ë
    private string _adUnitId = "ca-app-pub-5086433509319711/5903300775";    // Âð
#else
    private string _adUnitId = "unused";
#endif

    private RewardedAd rewardedAd;

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        AdRequest adRequest = new AdRequest.Builder().Build();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed, an error is returned.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            // If the operation completed successfully, no error is returned.
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

            // Create and pass the SSV options to the rewarded ad.
            var options = new ServerSideVerificationOptions
                                  .Builder()
                                  .SetCustomData("SAMPLE_CUSTOM_DATA_STRING")
                                  .Build();
            ad.SetServerSideVerificationOptions(options);

        });
        Debug.Log("Load Clear");
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
        Debug.Log("Show Clear");
    }

    public void DestroyFunc()
    {
        rewardedAd.Destroy();
        RegisterReloadHandler(rewardedAd);
        Debug.Log("Destroy Clear");
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        Debug.Log("Reload Clear");
    }
    #endregion
}

using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdMobManager : BehaviorSingleton<GoogleAdMobManager>
{
    #region Field
    /* 테스트 유무 */
    public static bool isTestMode = true;
    /* banner open-close */
    public bool isBannerOpen = false;

    #endregion

    #region Monobehaviour
    void Awake()
    {
        switch(PrefsBundle.isBannerOpen)
        {
            case 0:
                isBannerOpen = false;
                break;
            case 1:
                isBannerOpen = true;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(new List<string>() { "b74d8931364799ec" }) // test Device ID
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd();
    }
    #endregion

    #region check
    public bool CanShowAd()
    {
        if (rewardedAd == null) return false;
        return rewardedAd.CanShowAd();
    }
    #endregion

    #region 배너 광고
#if UNITY_ANDROID
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-5086433509319711/7826487033";
#elif UNITY_IPHONE
    const string bannerTestID = "ca-app-pub-3940256099942544/2934735716";
    const string bannerID = "ca-app-pub-5086433509319711/9283038937";
#else
    const string bannerTestID = "unused";
    const string bannerID = "unused";
#endif

    BannerView _bannerView;

    void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyAd();
        }

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView = new BannerView(isTestMode ? bannerTestID : bannerID,
            adaptiveSize, AdPosition.Bottom);
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        AdRequest adRequest = new AdRequest.Builder().Build();
        adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
        ToggleBannerAd();
    }

    public void ListenToAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };

        _bannerView.OnAdPaid += (AdValue adValue) =>
        { 
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };

        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };

        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };

        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    void DestroyAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    public void ToggleBannerAd()
    {
        if(_bannerView != null )
        {
            if (isBannerOpen) _bannerView.Show();
            else _bannerView.Hide();
        }
    }

    public void ToggleBannerAd(bool b)
    {
        isBannerOpen = b;
        PrefsBundle.Instance.SetInt(IntPrefs.isBannerOpen, b ? 1 : 0);
        ToggleBannerAd();
    }
    #endregion

    #region 리워드 광고
#if UNITY_ANDROID
    string rewardedTestID = "ca-app-pub-3940256099942544/5224354917";
    string rewardedID = "ca-app-pub-5086433509319711/4281105640";
#elif UNITY_IPHONE
    string rewardedTestID = "ca-app-pub-3940256099942544/1712485313";
    string rewardedID = "ca-app-pub-5086433509319711/5903300775";
#else
    string rewardedTestID = "unused";
    string rewardedID = "unused";
#endif

    RewardedAd rewardedAd;

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

        RewardedAd.Load(isTestMode ? rewardedTestID : rewardedID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
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

        RewardedAd.Load(isTestMode ? rewardedTestID : rewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());

            var options = new ServerSideVerificationOptions
                                  .Builder()
                                  .SetCustomData("SAMPLE_CUSTOM_DATA_STRING")
                                  .Build();
            ad.SetServerSideVerificationOptions(options);
        });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });

            RegisterEventHandlers(rewardedAd);
            RegisterReloadHandler(rewardedAd);
        }
    }

    void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            LoadRewardedAd();
        };
    }
    #endregion
}
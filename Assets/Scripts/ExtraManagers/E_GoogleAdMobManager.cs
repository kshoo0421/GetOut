using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEngine;

public class E_GoogleAdMobManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ »ý¼º¿ë
    private static E_GoogleAdMobManager instance;
    private E_GoogleAdMobManager() { }
    public static E_GoogleAdMobManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<E_GoogleAdMobManager>();
        }
    }
    #endregion
    public static bool isTestMode = false;

    private void Start()
    {
        var requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(new List<string>() { "b74d8931364799ec" }) // test Device ID
            .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd();
        LoadRewardedAd();
    }

    public bool CanShowAd()
    {
        if (rewardedAd == null) return false;
        return rewardedAd.CanShowAd();
    }

    private AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region ¹è³Ê ±¤°í
#if UNITY_ANDROID
    private const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    private const string bannerID = "ca-app-pub-5086433509319711/7826487033";
#elif UNITY_IPHONE
    private const string bannerTestID = "ca-app-pub-3940256099942544/2934735716";
    private const string bannerID = "ca-app-pub-5086433509319711/9283038937";
#else
    private const string bannerTestID = "unused";
    private const string bannerID = "unused";
#endif

    private BannerView bannerAd;

    private void LoadBannerAd()
    {
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
             adaptiveSize, AdPosition.Bottom);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion

    #region ¸®¿öµå ±¤°í
#if UNITY_ANDROID
    private string rewardedTestID = "ca-app-pub-3940256099942544/5224354917";
    private string rewardedID = "ca-app-pub-5086433509319711/4281105640";
#elif UNITY_IPHONE
    private string rewardedTestID = "ca-app-pub-3940256099942544/1712485313";
    private string rewardedID = "ca-app-pub-5086433509319711/5903300775";
#else
    private string rewardedTestID = "unused";
    private string rewardedID = "unused";
#endif
    private RewardedAd rewardedAd;

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest.Builder builder = new AdRequest.Builder();
        AdRequest adRequest = builder.Build();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(isTestMode ? rewardedTestID : rewardedID, adRequest,
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
        RewardedAd.Load(isTestMode ? rewardedTestID : rewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
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
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        LoadRewardedAd();
    }
    #endregion
}

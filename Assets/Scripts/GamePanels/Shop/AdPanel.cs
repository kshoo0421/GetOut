using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdPanel : MonoBehaviour
{
    #region Fields
    private GoogleAdMobManager googleAdMobManager;
    [SerializeField] Button RewardAdsBtn;
    [SerializeField] TMP_Text IsBannerOn;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        googleAdMobManager = GoogleAdMobManager.Instance;
        SetBannerText();
        SetAd();
    }

    private void Update()
    {
        CanShowAd();
        googleAdMobManager.ListenToAdEvents();
    }
    #endregion

    #region Ad Panel
    private void SetBannerText()
    {
        if (DatabaseManager.bannerAd)
        {
            IsBannerOn.text = "On";
            IsBannerOn.color = Color.blue;
        }
        else
        {
            IsBannerOn.text = "Off";
            IsBannerOn.color = Color.red;
        }
    }

    private void SetAd()
    {
        googleAdMobManager.LoadRewardedAd();
    }

    private void CanShowAd() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // 리워드 광고를 틀 수 있으면 활성화

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // 배너 광고 설정/해제

    public void ShowBannerAd()
    {
        googleAdMobManager.ToggleBannerAd(true);
        DatabaseManager.bannerAd = true;
        SetBannerText();
    }

    public void CloseBannerAd()
    {
        googleAdMobManager.ToggleBannerAd(false);
        DatabaseManager.bannerAd = false;
        SetBannerText();
    }


    public void ShowRewardedAd()
    {
        googleAdMobManager.ShowRewardedAd();
        DatabaseManager.userData.itemData.gold += 100;
    }
    #endregion

}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S08_Shop : Scenes
{
    #region Field
    /* Ads */
    [SerializeField] private Button[] AdsBtns;  // 0 : 배너 광고, 
    [SerializeField] private Button RewardAdsBtn;

    /* CurState */
    [SerializeField] private TMP_Text curStateTmp;

    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        SetAd();
    }

    private void Update()
    {
        ForUpdate();
        CanShowAd();
        googleAdMobManager.ListenToAdEvents();
    }
    #endregion

    #region Ads
    private void SetAd()
    {
        googleAdMobManager.LoadRewardedAd();
    }

    private void CanShowAd() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // 리워드 광고를 틀 수 있으면 활성화

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // 배너 광고 설정/해제

    public void ShowRewardedAd() => googleAdMobManager.ShowRewardedAd();

    #endregion

    #region Purchase
    public string targetProductId;

    public void HandleClick()
    {
        if (targetProductId == PaymentManager.ProductCharacterSkin
            || targetProductId == PaymentManager.ProductSubscription)
        {
            if (paymentManager.HadPurchased(targetProductId))
            {
                Debug.Log("이미 구매한 상품");
                return;
            }
        }

        paymentManager.Purchase(targetProductId);
    }

    #endregion
}
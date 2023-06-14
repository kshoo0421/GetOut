using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S09_Shop : Scenes
{
    #region Field
    /* Ads */
    [SerializeField] Button[] AdsBtns;  // 0 : ��� ����, 
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

    #region Ads
    void SetAd()
    {
        googleAdMobManager.ToggleBannerAd();
        googleAdMobManager.LoadRewardedAd();
    }
    void CanShowAd() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // ������ ���� Ʋ �� ������ Ȱ��ȭ

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // ��� ���� ����/����

    public void ShowRewardedAd() => googleAdMobManager.ShowRewardedAd();

    #endregion

    #region Purchase
    public string targetProductId;

    public void HandleClick()
    {
        if(targetProductId == PaymentManager.ProductCharacterSkin
            || targetProductId == PaymentManager.ProductSubscription)
        {
            if(paymentManager.HadPurchased(targetProductId))
            {
                Debug.Log("�̹� ������ ��ǰ");
                return;
            }
        }

        paymentManager.Purchase(targetProductId);
    }

    #endregion
}
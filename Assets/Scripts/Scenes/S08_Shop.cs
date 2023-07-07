using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S08_Shop : Scenes
{
    #region Field
    /* Ads */
    [SerializeField] private Button[] AdsBtns;  // 0 : ��� ����, 
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

    private void CanShowAd() => RewardAdsBtn.interactable = googleAdMobManager.CanShowAd(); // ������ ���� Ʋ �� ������ Ȱ��ȭ

    public void ToggleBannerAd(bool b) => googleAdMobManager.ToggleBannerAd(b); // ��� ���� ����/����

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
                Debug.Log("�̹� ������ ��ǰ");
                return;
            }
        }

        paymentManager.Purchase(targetProductId);
    }

    #endregion
}
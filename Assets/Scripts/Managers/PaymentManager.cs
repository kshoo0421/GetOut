using UnityEngine;
using UnityEngine.Purchasing;

public class PaymentManager : BehaviorSingleton<PaymentManager>, IStoreListener
{
    #region Field
    /* strings - ID*/
    public const string ProductGold = "gold";   // Consumable
    public const string ProductCharacterSkin = "character_skin"; // Unconsumable
    public const string ProductSubscription = "premium_subscription"; // Subscription

    private const string _iOS_GoldId = "com.studio.app.gold";
    private const string _android_GoldId = "com.studio.app.gold";

    private const string _iOS_SkinId = "com.studio.app.skin";
    private const string _android_SkinId = "com.studio.app.skin";

    private const string _iOS_PremiumId = "com.studio.app.sub";
    private const string _android_PremiumId = "com.studio.app.sub";
    /* for Interface - IStoreLister */
    private IStoreController storeController; // 구매 과정을 제어하는 함수를 제공
    private IExtensionProvider storeExtensionProvider;  // 여러 플랫폼을 위한 확장 처리를 제공
    #endregion

    #region Monobehaviour
    void Awake() => InitUnityIAP();
    #endregion

    #region Initialize
    void InitUnityIAP()
    {
        if (IsInitialized) return;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(
            ProductGold, ProductType.Consumable, new IDs() {
                    { _iOS_GoldId, AppleAppStore.Name},
                    {_android_GoldId, GooglePlay.Name }
                }
             );

        builder.AddProduct(
            ProductCharacterSkin, ProductType.NonConsumable, new IDs() {
                    {_iOS_SkinId, AppleAppStore.Name},
                    {_android_SkinId, GooglePlay.Name }
                }
             );

        builder.AddProduct(
            ProductSubscription, ProductType.Subscription, new IDs() {
                    {_iOS_PremiumId, AppleAppStore.Name},
                    {_android_PremiumId, GooglePlay.Name }
                }
             );

        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;
    #endregion

    #region Interface - IStoreListener
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error}");
    }
    public void OnInitializeFailed(InitializationFailureReason error, string str)   // Not in Youtube Video
    {
        Debug.LogError($"유니티 IAP 초기화 실패 {error} \n {str}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");
        if(args.purchasedProduct.definition.id == ProductGold)
        {
            Debug.Log("골드 상승 처리...");
        }
        else if (args.purchasedProduct.definition.id == ProductCharacterSkin)
        {
            Debug.Log("스킨 등록...");
        }
        else if (args.purchasedProduct.definition.id == ProductSubscription)
        {
            Debug.Log("구독 서비스 시작...");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }
    #endregion

    #region Purchase
    public void Purchase(string productId)
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);
        
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"구매 시도 - {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"구매 시도 불가 - {productId}");
        }
    }

    public void ResotrePurchase()   // 구매 복구 함수. 안드로이드는 필요 없음. iOS는 필요
    {
        if(!IsInitialized) return;

        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)   // iPhone, 맥킨토시
        {
            Debug.Log("구매 복구 시도");

            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();

            appleExt.RestoreTransactions(
                    result => Debug.Log($"구매 복구 시도 결과 - {result}")); // 기존 구매 내역 복구
        }
    }

    public bool HadPurchased(string productId)
    {
        if(!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if(product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }
    #endregion
}

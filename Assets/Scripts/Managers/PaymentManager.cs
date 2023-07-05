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
    private IStoreController storeController; // ���� ������ �����ϴ� �Լ��� ����
    private IExtensionProvider storeExtensionProvider;  // ���� �÷����� ���� Ȯ�� ó���� ����
    #endregion

    #region Monobehaviour
    private void Awake() => InitUnityIAP();
    #endregion

    #region Initialize
    private void InitUnityIAP()
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
        Debug.Log("����Ƽ IAP �ʱ�ȭ ����");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}");
    }
    public void OnInitializeFailed(InitializationFailureReason error, string str)   // Not in Youtube Video
    {
        Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error} \n {str}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"���� ���� - ID : {args.purchasedProduct.definition.id}");
        if (args.purchasedProduct.definition.id == ProductGold)
        {
            Debug.Log("��� ��� ó��...");
        }
        else if (args.purchasedProduct.definition.id == ProductCharacterSkin)
        {
            Debug.Log("��Ų ���...");
        }
        else if (args.purchasedProduct.definition.id == ProductSubscription)
        {
            Debug.Log("���� ���� ����...");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogWarning($"���� ���� - {product.definition.id}, {reason}");
    }
    #endregion

    #region Purchase
    public void Purchase(string productId)
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"���� �õ� - {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"���� �õ� �Ұ� - {productId}");
        }
    }

    public void ResotrePurchase()   // ���� ���� �Լ�. �ȵ���̵�� �ʿ� ����. iOS�� �ʿ�
    {
        if (!IsInitialized) return;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)   // iPhone, ��Ų���
        {
            Debug.Log("���� ���� �õ�");

            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();

            appleExt.RestoreTransactions(
                    result => Debug.Log($"���� ���� �õ� ��� - {result}")); // ���� ���� ���� ����
        }
    }

    public bool HadPurchased(string productId)
    {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject ResultPanel;

    private PaymentManager paymentManager;
    private string targetProductId;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        paymentManager = PaymentManager.Instance;
    }
    #endregion

    #region Functions
    public void HandleClick()   // temp
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

    public void Product10000()
    {
        targetProductId = "10000";
        paymentManager.Purchase(targetProductId);
        DatabaseManager.userData.itemData.gold += 10000;
        DatabaseManager.Instance.SaveItemData();
        OpenResultPanel();
    }

    public void Product33000()
    {
        targetProductId = "33000";
        paymentManager.Purchase(targetProductId);
        DatabaseManager.userData.itemData.gold += 33000;
        DatabaseManager.Instance.SaveItemData();
        OpenResultPanel();
    }

    public void Product60000()
    {
        targetProductId = "60000";
        paymentManager.Purchase(targetProductId);
        DatabaseManager.userData.itemData.gold += 60000;
        DatabaseManager.Instance.SaveItemData();
        OpenResultPanel();
    }

    public void Product150000()
    {
        targetProductId = "150000";
        paymentManager.Purchase(targetProductId);
        DatabaseManager.userData.itemData.gold += 150000;
        DatabaseManager.Instance.SaveItemData();
        OpenResultPanel();
    }

    private void OpenResultPanel()
    {
        ResultPanel.SetActive(true);
    }

    public void CloseResultPanel()
    {
        ResultPanel.SetActive(false);
    }
    #endregion
}

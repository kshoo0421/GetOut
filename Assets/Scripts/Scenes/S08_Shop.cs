using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S08_Shop : Scenes
{
    #region monobehaviour
    private void Start()
    {
        InitialSet();
        OpenTicketPanel();
    }

    private void Update()
    {
        ForUpdate();
    }
    #endregion

    #region Panels
    [SerializeField] private GameObject TicketPanel;
    [SerializeField] private GameObject AdPanel;
    [SerializeField] private GameObject PurchasePanel;

    public void OpenTicketPanel()
    {
        TicketPanel.SetActive(true);
        PurchasePanel.SetActive(false);
        AdPanel.SetActive(false);
    }

    public void OpenAdPanel()
    {
        TicketPanel.SetActive(false);
        PurchasePanel.SetActive(false);
        AdPanel.SetActive(true);
    }

    public void OpenPurchasePanel()
    {
        TicketPanel.SetActive(false);
        AdPanel.SetActive(false);
        PurchasePanel.SetActive(true);
    }
    #endregion
}
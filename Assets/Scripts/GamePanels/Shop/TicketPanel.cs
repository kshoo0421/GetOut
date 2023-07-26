using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Purchasing;

public class TicketPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_InputField TicketCount;
    [SerializeField] private TMP_Text PriceAmountText;
    [SerializeField] private TMP_Text CurrentExtraTicketText;
    [SerializeField] private GameObject ConfirmPanel;
    [SerializeField] private GameObject ResultPanel;
    #endregion

    #region Monobehabiour
    private void OnEnable()
    {
        SetText();
    }
    #endregion

    #region Functions
    public void SetText()
    {
        if (OptionManager.curLocale == 0)
        {
            PriceAmountText.text = "Price : 0 Gold";
            CurrentExtraTicketText.text = $"Current Extra Tickets : {DatabaseManager.userData.itemData.extraTicket}";
        }
        else
        {
            PriceAmountText.text = "가격 : 0 골드";
            CurrentExtraTicketText.text = $"현재 여분 티켓 : {DatabaseManager.userData.itemData.extraTicket}";
        }
    }

    public void UpdateTicketCount()
    {
        if(TicketCount.text == "")
        {
            SetText();
            return;
        }

        long curGold = DatabaseManager.userData.itemData.gold;
        long curString = long.Parse(TicketCount.text) * 1000;
        if (curGold < curString)
        {
            TicketCount.text = (curGold / 1000).ToString();
        }
        UpdatePriceAmountTMP();
    }

    private void UpdatePriceAmountTMP()
    {
        string price = (long.Parse(TicketCount.text) * 1000).ToString();
        if (OptionManager.curLocale == 0)
        {
            PriceAmountText.text = "Price : " + price + " Gold";
        }
        else
        {
            PriceAmountText.text = "가격 : " + price + " 골드";
        }
    }

    public void OpenConfirmPanel() => ConfirmPanel.SetActive(true);

    public void CloseConfirmPanel() => ConfirmPanel.SetActive(false);
    #endregion
}

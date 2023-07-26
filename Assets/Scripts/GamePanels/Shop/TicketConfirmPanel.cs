using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TicketConfirmPanel : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text ConfirmText;
    [SerializeField] private TMP_InputField ExtraTicketText;
    [SerializeField] private GameObject ResultPanel;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SetConfirmText();
    }
    #endregion

    #region Functions
    private void SetConfirmText()
    {
        Debug.Log("type : " + ExtraTicketText.text.GetType());
        int gold = int.Parse(ExtraTicketText.text) * 1000;
        if(OptionManager.curLocale == 0)
        {
            ConfirmText.text = $"Do you want to buy {ExtraTicketText.text} extra ticket(s)?\n" +
                $"(Price : {gold} Gold)";
        }
        else
        {
            ConfirmText.text = $"���� Ƽ�� {ExtraTicketText.text}���� �����Ͻðڽ��ϱ�?\n" +
                $"(���� : {gold} ���)";
        }
    }

    public void BuyExtraTicket()
    {
        long extraTicket = long.Parse(ExtraTicketText.text);
        DatabaseManager.userData.itemData.extraTicket += extraTicket;
        DatabaseManager.userData.itemData.gold -= extraTicket * 1000;
        ExtraTicketText.text = "";
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

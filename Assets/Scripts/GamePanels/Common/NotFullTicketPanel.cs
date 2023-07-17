using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotFullTicketPanel : MonoBehaviour
{
    #region Fields
    private int extraTicketCount;
    private int selectCount;
    [SerializeField] private TMP_Text extraTicketCountTMP;
    [SerializeField] private TMP_Text maxExtraticketCountTMP;
    [SerializeField] private TMP_Text selectCountTMP;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TMP_Text confirmInfoText;
    [SerializeField] private TMP_Text[] confirmTexts;

    [SerializeField] private GameObject beforeChargePanel;
    [SerializeField] private GameObject afterChargePanel;
    #endregion

    #region Mpnobehaviour
    private void OnEnable()
    {
        init();
    }

    private void Update()
    {
        selectCountTMP.text = selectCount.ToString();
    }
    #endregion

    #region Init
    private void init()
    {
        beforeChargePanel.SetActive(true);
        afterChargePanel.SetActive(false);

        extraTicketCount = (int)DatabaseManager.userData.itemData.extraTicket;
        extraTicketCountTMP.text = $"You have {extraTicketCount} Extra Ticket";
        int max = 5 - (int)DatabaseManager.userData.itemData.ticket;

        if (extraTicketCount > max)
        {
            maxExtraticketCountTMP.text = max.ToString();
            slider.maxValue = max;
        }
        else
        {
            maxExtraticketCountTMP.text = extraTicketCount.ToString();
            slider.maxValue = extraTicketCount;
        }
        selectCount = 0;
    }
    #endregion

    #region slider
    public void SelectTicketCount(float amount)
    {
        int i = (int)amount;
        if (amount - i >= 0.5)
        {
            i++;
        }
        selectCount = i;
    }
    #endregion

    #region Panel Control
    public void OpenConfirmPanel()
    {
        confirmPanel.SetActive(true);
        int curTicket = (int)DatabaseManager.userData.itemData.ticket;
        confirmInfoText.text = $"Do you really exchange {selectCount} extra ticket to ticket?";
        confirmTexts[0].text = extraTicketCount.ToString();
        confirmTexts[1].text = (extraTicketCount - selectCount).ToString();
        confirmTexts[2].text = curTicket.ToString();
        confirmTexts[3].text = (curTicket + selectCount).ToString();
    }

    public void ConfirmYesBtn()
    {
        DatabaseManager.userData.itemData.ticket += (long)selectCount;
        DatabaseManager.userData.itemData.extraTicket -= (long)selectCount;
        if (DatabaseManager.userData.itemData.ticket == 5)
        {
            DatabaseManager.restMinute = "00";
            DatabaseManager.restSecond = "00";
        }
        slider.value = 0f;
        DatabaseManager.Instance.SaveItemData();
        beforeChargePanel.SetActive(false);
        afterChargePanel.SetActive(true);
        CloseConfirmPanel();
    }


    public void CloseConfirmPanel()
    {
        confirmPanel.SetActive(false);
    }
    #endregion
}
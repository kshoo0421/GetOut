using TMPro;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    #region Fields
    private DatabaseManager databaseManager;
    // Option Panel
    [SerializeField] private GameObject OptionPanel;

    // nick name
    [SerializeField] private TMP_Text nameText;
    
    // ticket
    [SerializeField] private TMP_Text ticketCount;
    [SerializeField] private TMP_Text restTime;

    // plus ticket
    [SerializeField] private GameObject plusTicketPanel;

    // gold
    [SerializeField] private TMP_Text goldAmount;
    [SerializeField] private GameObject plusGoldPanel;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        databaseManager = DatabaseManager.Instance;
        ShowNickNameText();
        restTime.text = "00:00";
    }

    private void Update()
    {
        UpdateTicketText();
        UpdateGoldText();
    }

    private void OnDestroy()
    {
        restTime.text = "00:00";
    }
    #endregion
    
    #region nickName
    public void ShowNickNameText()
    {
        if (databaseManager.GetCurUser() != null)
        {
            if(OptionManager.curLocale == 0)
            {
                nameText.text = $"User : {DatabaseManager.userData.nickName}";
            }
            else
            {
                nameText.text = $"»ç¿ëÀÚ : {DatabaseManager.userData.nickName}";
            }
        }
        else
        {
            nameText.text = "ERROR : null";
        }
    }
    #endregion

    #region Text Update
    private void UpdateTicketText()
    {
        ticketCount.text = DatabaseManager.userData.itemData.ticket.ToString();
        restTime.text = DatabaseManager.restMinute + ":" + DatabaseManager.restSecond;
    }

    private void UpdateGoldText()
    {
        goldAmount.text = DatabaseManager.userData.itemData.gold.ToString();
    }
    #endregion

    #region TogglePanels
    public void OpenOptionPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        OptionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        OptionPanel.SetActive(false);
    }

    public void OpenPlusTicketPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        plusTicketPanel.SetActive(true);
    }

    public void ClosePlusTicketPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        plusTicketPanel.SetActive(false);
    }

    public void OpenPlusGoldPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        plusGoldPanel.SetActive(true);
    }

    public void ClosePlusGoldPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
        plusGoldPanel.SetActive(false);
    }
    #endregion

    #region Option Panel

    #endregion
}

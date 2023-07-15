using TMPro;
using UnityEngine;

public class S01_Lobby : Scenes
{
    #region Field
    /* Check Sign In*/
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text ticketCount;
    [SerializeField] private TMP_Text restTime;
    [SerializeField] private TMP_Text extraTicketCount;
    [SerializeField] private GameObject BackBtnPanel;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        ShowNickNameText();
    }

    private void Update()
    {
        ForUpdate();
        ticketCount.text = DatabaseManager.userData.itemData.ticket.ToString();
        extraTicketCount.text = DatabaseManager.userData.itemData.extraTicket.ToString();
        restTime.text = DatabaseManager.restMinute + ":" + DatabaseManager.restSecond;
    }

    private void OnDestroy()
    {
        restTime.text = "00:00";
    }
    #endregion

    #region Sign Out
    public void SignOut()
    {
        databaseManager.SignOut();
        ChangeToScene(0);
    }
    #endregion

    #region Btn
    public void ToggleBackBtnPanel()
    {
        if (BackBtnPanel.activeSelf)
        {
            BackBtnPanel.SetActive(false);
        }
        else
        {
            BackBtnPanel.SetActive(true);
        }
    }

    #endregion

    #region test1
    public void ShowNickNameText()
    {
        if (databaseManager.GetCurUser() != null)
        {
            nameText.text = $"{databaseManager.GetCurUser().UserId}";
        }
        else
        {
            nameText.text = "ERROR : null";
        }
    }
    #endregion

    #region test2
    public void Test()    // 데이터베이스 테스트용
    {
        // ResultDataSave();
        // UserDataSave();
        MinusTicket();
    }

    private void MinusTicket()
    {
        if (databaseManager.CanUseTicket()) databaseManager.UseTicket();
    }
    #endregion
}
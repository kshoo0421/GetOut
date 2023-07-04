using UnityEngine;

using TMPro;

public class S01_Lobby : Scenes
{
    #region Field
    /* Check Sign In*/
    [SerializeField] TMP_Text ticketCount;
    [SerializeField] TMP_Text restTime;
    [SerializeField] TMP_Text extraTicketCount;
    [SerializeField] GameObject BackBtnPanel;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        ShowHiText();
    }

    void Update()
    {
        ForUpdate();
        ticketCount.text = DatabaseManager.userData.itemData.ticket.ToString();
        extraTicketCount.text = DatabaseManager.userData.itemData.extraTicket.ToString();
        restTime.text = DatabaseManager.restMinute + ":" + DatabaseManager.restSecond;
    }

    void OnDestroy()
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
        if(BackBtnPanel.activeSelf) 
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
    [SerializeField] TMP_Text nameText;
    public void ShowHiText()
    {
        if (databaseManager.GetCurUser() != null)
        {
            nameText.text = $"Hi! {databaseManager.GetCurUser().Email}";
        }
        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }
    #endregion

    #region test2
    public new void Test()    // 데이터베이스 테스트용
    {
        // ResultDataSave();
        // UserDataSave();
        // MinusTicket();
    }

    void MinusTicket()
    {
        if(databaseManager.CanUseTicket()) databaseManager.UseTicket();
    }
    #endregion
}
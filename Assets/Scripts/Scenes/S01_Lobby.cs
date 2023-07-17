using TMPro;
using UnityEngine;

public class S01_Lobby : Scenes
{
    #region Field
    [SerializeField] private GameObject BackBtnPanel;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
    }

    private void Update()
    {
        ForUpdate();
    }

    #endregion

    #region Sign Out
    public void SignOut()
    {
        databaseManager.SignOut();
        ChangeToScene("Init");
    }
    #endregion

    #region Btn
    public void ToggleBackBtnPanel()
    {
        SoundEffectManager.PlaySound(Sound.Button);
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
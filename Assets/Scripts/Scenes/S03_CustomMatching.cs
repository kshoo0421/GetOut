using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class S03_CustomMatching : Scenes
{
    #region Field
    private int curUserNum;

    [SerializeField] private TMP_Text roomNumber; // room number
    [SerializeField] private TMP_Text curState; // current state
    [SerializeField] private TMP_Text[] playerNames;
    [SerializeField] private TMP_Text[] playerOX;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        databaseManager.InitGameData();
        roomNumber.text = PhotonManager.RoomString;
        DatabaseManager.randomOrCustom = RandomOrCustom.Custom;
    }

    private void Update()
    {
        ForUpdate();
        UpdatePlayerNickName();
        UpdatePlayerReady();
    }
    #endregion

    #region Update String
    private void UpdatePlayerNickName()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length)
            {
                playerNames[i].text = photonManager.GetPlayerInformation(i);
            }
            else
            {
                playerNames[i].text = "AI";
            }
        }
    }
    #endregion

    #region Check Ready
    public void UpdatePlayerReady()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length)
            {
                playerOX[i].text = DatabaseManager.gameData.playerReady[i] ? "O" : "X";
            }
            else
            {
                playerOX[i].text = " ";
            }
        }
    }
    #endregion

    #region Button
    public void ToggleReady()
    {
        GamePlayer gamePlayer = DatabaseManager.MyPlayer;
        gamePlayer.ToggleGameReady();
    }

    #endregion

    #region Panels
    [SerializeField] private GameObject NotReadyPanel;
    [SerializeField] private GameObject BackToWaitingPanel;

    private void ToggleNotReadyPanel(bool b) => NotReadyPanel.SetActive(b);

    private void ToggleBackToWaitingPanel(bool b) => BackToWaitingPanel.SetActive(b);

    public void CloseNotReadyPanel()
    {
        ToggleNotReadyPanel(false);
    }

    public void OpenBackToWaitingPanel()
    {
        ToggleBackToWaitingPanel(true);
    }

    public void CloseBackToWaitingPanel()
    {
        ToggleBackToWaitingPanel(false);
    }


    #endregion

    #region GameStart
    private bool CheckAllPlayerReady()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (DatabaseManager.gameData.playerReady[i] == false) 
                return false;
        }
        return true;
    }

    public void GameStart()
    {
        if (CheckAllPlayerReady())
        {
            DatabaseManager.MyPlayer = null;
            ChangeToScene(5);
//            photonManager.LoadScene(5);
        }
        else
        {
            ToggleNotReadyPanel(true);
        }
    }

    public void BackToWaitingRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        photonManager.LeaveRoom();
        ChangeToScene(2);
    }
    #endregion

    #region Test
    public void Test()
    {
    }
    #endregion
}
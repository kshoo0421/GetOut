using UnityEngine;
using TMPro;
using Photon.Pun;

public class S03_CustomMatching : Scenes
{
    #region Field
    int curUserNum;

    [SerializeField] TMP_Text roomNumber; // room number
    [SerializeField] TMP_Text curState; // current state
    [SerializeField] TMP_Text[] playerNames;
    [SerializeField] TMP_Text[] playerOX;
    #endregion

    #region monobehaviour
    void Start()
    { 
        InitialSet();
        roomNumber.text = PhotonManager.RoomString;
    }
    void Update()
    {
        ForUpdate();
        UpdatePlayerNickName();
        UpdatePlayerReady();
    }
    #endregion

    #region Update String
    void UpdatePlayerNickName()
    {
        for(int i = 0; i < 4; i++)
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
        for(int i = 0; i < 4; i++)
        {
            if(i <  PhotonNetwork.PlayerList.Length)
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
        GamePlayer gamePlayer = DatabaseManager.MyPlayer.GetComponent<GamePlayer>();
        gamePlayer.TogglePlayerReady();
    }

    #endregion

    #region Panels
    [SerializeField] GameObject NotReadyPanel;
    [SerializeField] GameObject BackToWaitingPanel;

    void ToggleNotReadyPanel(bool b) => NotReadyPanel.SetActive(b);

    void ToggleBackToWaitingPanel(bool b) => BackToWaitingPanel.SetActive(b);
    
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
    bool CheckAllPlayerReady()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (DatabaseManager.gameData.playerReady[i] == false) return false;
        }
        return true;
    }

    public void GameStart()
    {
        if(CheckAllPlayerReady())
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(6);
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
}
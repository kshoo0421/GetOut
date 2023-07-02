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
                playerOX[i].text = FirebaseManager.gameData.playerReady[i] ? "O" : "X";
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
        GamePlayer gamePlayer = FirebaseManager.MyPlayer.GetComponent<GamePlayer>();
        gamePlayer.TogglePlayerReady();
    }

    #endregion

    #region GameStart
    public void GameStart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(6);
        ChangeToScene(6);
    }

    public void BackToWaitingRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        photonManager.LeaveRoom();
        ChangeToScene(2);
    }
    #endregion
}
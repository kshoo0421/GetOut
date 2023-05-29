using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_LobbyManager : MonoBehaviourPunCallbacks
{
    #region Field
    /* Managers */
    TotalGameManager totalGameManager;
    B_SceneChangeManager sceneChanger;

    /* Photon */
    readonly string gameVersion = "1";
    FirebaseManager firebaseManager;
    TMP_Text connectionInfoText;
    Button joinButton;
    #endregion

    #region monobehaviour
    void Start()
    {
        SetManagers();
        SetPhoton();
    }
    #endregion

    #region Set Managers
    void SetManagers()
    {
        totalGameManager = TotalGameManager.Instance;
        sceneChanger = totalGameManager.b_SceneChangeManager;
        firebaseManager = totalGameManager.firebaseManager;
    }
    #endregion

    #region Change Scene  
    public void ChangeToScene(int sceneIndex) => sceneChanger.ChangetoScene(sceneIndex);
    #endregion

    #region Photon
    void SetPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
        TestFunc();
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected To Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Conection Disabled {cause.ToString()} - Try reconnecting...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = $"Offline : Conection Disabled - Try reconnecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";
        PhotonNetwork.LoadLevel("06_Game");
    }

    public void SignOut()
    {
        firebaseManager.SignOut();
        sceneChanger.ChangetoScene(1);
    }
    #endregion

    #region test1
    public TMP_Text nameText;
    public void TestFunc()
    {
        Debug.Log("Test");
        if (firebaseManager.GetCurUser() != null)
        {
            Debug.Log(firebaseManager.GetCurUser().Email);
            nameText.text = $"Hi! {firebaseManager.GetCurUser().Email}";
        }
        else
        {
            nameText.text = "ERROR : AuthManager.User == null";
        }
    }
    #endregion

    #region test2
    public void MakeResultData()    // 데이터베이스 테스트용
    {
        ResultData resultData = new ResultData();
        resultData.GAME_KEY = "123456789";

        resultData.finalResult.first = 2;
        resultData.finalResult.second = 1;
        resultData.finalResult.third = 4;
        resultData.finalResult.fourth = 3;

        resultData.turn1.player1.isGet = true;
        resultData.turn1.player1.value = 50;
        resultData.turn1.player2.isGet = true;
        resultData.turn1.player2.value = 50;
        resultData.turn1.player3.isGet = true;
        resultData.turn1.player3.value = 50;
        resultData.turn1.player4.isGet = true;
        resultData.turn1.player4.value = 50;

        resultData.turn2.player1.isGet = true;
        resultData.turn2.player1.value = 50;
        resultData.turn2.player2.isGet = true;
        resultData.turn2.player2.value = 50;
        resultData.turn2.player3.isGet = true;
        resultData.turn2.player3.value = 50;
        resultData.turn2.player4.isGet = true;
        resultData.turn2.player4.value = 50;

        resultData.turn3.player1.isGet = true;
        resultData.turn3.player1.value = 50;
        resultData.turn3.player2.isGet = true;
        resultData.turn3.player2.value = 50;
        resultData.turn3.player3.isGet = true;
        resultData.turn3.player3.value = 50;
        resultData.turn3.player4.isGet = true;
        resultData.turn3.player4.value = 50;

        resultData.turn4.player1.isGet = true;
        resultData.turn4.player1.value = 50;
        resultData.turn4.player2.isGet = true;
        resultData.turn4.player2.value = 50;
        resultData.turn4.player3.isGet = true;
        resultData.turn4.player3.value = 50;
        resultData.turn4.player4.isGet = true;
        resultData.turn4.player4.value = 50;

        resultData.turn5.player1.isGet = true;
        resultData.turn5.player1.value = 50;
        resultData.turn5.player2.isGet = true;
        resultData.turn5.player2.value = 50;
        resultData.turn5.player3.isGet = true;
        resultData.turn5.player3.value = 50;
        resultData.turn5.player4.isGet = true;
        resultData.turn5.player4.value = 50;

        resultData.turn6.player1.isGet = true;
        resultData.turn6.player1.value = 50;
        resultData.turn6.player2.isGet = true;
        resultData.turn6.player2.value = 50;
        resultData.turn6.player3.isGet = true;
        resultData.turn6.player3.value = 50;
        resultData.turn6.player4.isGet = true;
        resultData.turn6.player4.value = 50;

        resultData.player1.playerName = "User1";
        resultData.player1.playerId = "Test1";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User2";
        resultData.player1.playerId = "Test2";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User3";
        resultData.player1.playerId = "Test3";
        resultData.player1.curGold = 200;

        resultData.player1.playerName = "User4";
        resultData.player1.playerId = "Test4";
        resultData.player1.curGold = 200;

        resultData.player1Mission.low.missionNum = 1;
        resultData.player1Mission.low.isAchieved = true;
        resultData.player1Mission.mid.missionNum = 1;
        resultData.player1Mission.mid.isAchieved = true;
        resultData.player1Mission.high.missionNum = 1;
        resultData.player1Mission.high.isAchieved = false;

        resultData.player2Mission.low.missionNum = 1;
        resultData.player2Mission.low.isAchieved = true;
        resultData.player2Mission.mid.missionNum = 1;
        resultData.player2Mission.mid.isAchieved = true;
        resultData.player2Mission.high.missionNum = 1;
        resultData.player2Mission.high.isAchieved = false;

        resultData.player3Mission.low.missionNum = 1;
        resultData.player3Mission.low.isAchieved = true;
        resultData.player3Mission.mid.missionNum = 1;
        resultData.player3Mission.mid.isAchieved = true;
        resultData.player3Mission.high.missionNum = 1;
        resultData.player3Mission.high.isAchieved = false;

        resultData.player4Mission.low.missionNum = 1;
        resultData.player4Mission.low.isAchieved = true;
        resultData.player4Mission.mid.missionNum = 1;
        resultData.player4Mission.mid.isAchieved = true;
        resultData.player4Mission.high.missionNum = 1;
        resultData.player4Mission.high.isAchieved = false;

        firebaseManager.SaveData(resultData);
    }
    #endregion
}

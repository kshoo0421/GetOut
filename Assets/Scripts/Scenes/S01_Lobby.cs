using UnityEngine;
using Firebase.Auth;
using Photon.Realtime;
using TMPro;

public class S01_Lobby : Scenes
{
    #region Field
    /* Check Sign In*/
    FirebaseUser curUser;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        TestFunc();
    }
    #endregion

    #region Photon
    public void OnConnectedToMaster() => photonManager.OnConnectedToMaster();

    public void OnDisconnected(DisconnectCause cause) => photonManager.OnDisconnected(cause);

    public void Connect() => photonManager.Connect();

    public void OnJoinRandomFailed(short returnCode, string message) => photonManager.OnJoinRandomFailed(returnCode, message);

    public void OnJoinedRoom() => photonManager.OnJoinedRoom();
    #endregion

    #region Sign Out
    public void SignOut()
    {
        firebaseManager.SignOut();
        ChangeToScene(0);
    }
    #endregion

    #region test1
    [SerializeField] TMP_Text nameText;
    public void TestFunc()
    {
        if (firebaseManager.GetCurUser() != null)
        {
            nameText.text = $"Hi! {firebaseManager.GetCurUser().Email}";
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
        UserDataSave();
    }

    void ResultDataSave()
    {
        ResultData resultData = new ResultData();

        resultData.finalResult.first = 2;
        resultData.finalResult.second = 1;
        resultData.finalResult.third = 4;
        resultData.finalResult.fourth = 3;

        resultData.turns.turn1.player1.isGet = true;
        resultData.turns.turn1.player1.value = 50;
        resultData.turns.turn1.player2.isGet = true;
        resultData.turns.turn1.player2.value = 50;
        resultData.turns.turn1.player3.isGet = true;
        resultData.turns.turn1.player3.value = 50;
        resultData.turns.turn1.player4.isGet = true;
        resultData.turns.turn1.player4.value = 50;

        resultData.turns.turn2.player1.isGet = true;
        resultData.turns.turn2.player1.value = 50;
        resultData.turns.turn2.player2.isGet = true;
        resultData.turns.turn2.player2.value = 50;
        resultData.turns.turn2.player3.isGet = true;
        resultData.turns.turn2.player3.value = 50;
        resultData.turns.turn2.player4.isGet = true;
        resultData.turns.turn2.player4.value = 50;

        resultData.turns.turn3.player1.isGet = true;
        resultData.turns.turn3.player1.value = 50;
        resultData.turns.turn3.player2.isGet = true;
        resultData.turns.turn3.player2.value = 50;
        resultData.turns.turn3.player3.isGet = true;
        resultData.turns.turn3.player3.value = 50;
        resultData.turns.turn3.player4.isGet = true;
        resultData.turns.turn3.player4.value = 50;

        resultData.turns.turn4.player1.isGet = true;
        resultData.turns.turn4.player1.value = 50;
        resultData.turns.turn4.player2.isGet = true;
        resultData.turns.turn4.player2.value = 50;
        resultData.turns.turn4.player3.isGet = true;
        resultData.turns.turn4.player3.value = 50;
        resultData.turns.turn4.player4.isGet = true;
        resultData.turns.turn4.player4.value = 50;

        resultData.turns.turn5.player1.isGet = true;
        resultData.turns.turn5.player1.value = 50;
        resultData.turns.turn5.player2.isGet = true;
        resultData.turns.turn5.player2.value = 50;
        resultData.turns.turn5.player3.isGet = true;
        resultData.turns.turn5.player3.value = 50;
        resultData.turns.turn5.player4.isGet = true;
        resultData.turns.turn5.player4.value = 50;

        resultData.turns.turn6.player1.isGet = true;
        resultData.turns.turn6.player1.value = 50;
        resultData.turns.turn6.player2.isGet = true;
        resultData.turns.turn6.player2.value = 50;
        resultData.turns.turn6.player3.isGet = true;
        resultData.turns.turn6.player3.value = 50;
        resultData.turns.turn6.player4.isGet = true;
        resultData.turns.turn6.player4.value = 50;

        resultData.players.player1.playerName = "User1";
        resultData.players.player1.playerId = "Test1";
        resultData.players.player1.curGold = 200;

        resultData.players.player1.playerName = "User2";
        resultData.players.player1.playerId = "Test2";
        resultData.players.player1.curGold = 200;

        resultData.players.player1.playerName = "User3";
        resultData.players.player1.playerId = "Test3";
        resultData.players.player1.curGold = 200;

        resultData.players.player1.playerName = "User4";
        resultData.players.player1.playerId = "Test4";
        resultData.players.player1.curGold = 200;

        resultData.missions.player1Mission.low.missionNum = 1;
        resultData.missions.player1Mission.low.isAchieved = true;
        resultData.missions.player1Mission.mid.missionNum = 1;
        resultData.missions.player1Mission.mid.isAchieved = true;
        resultData.missions.player1Mission.high.missionNum = 1;
        resultData.missions.player1Mission.high.isAchieved = false;

        resultData.missions.player2Mission.low.missionNum = 1;
        resultData.missions.player2Mission.low.isAchieved = true;
        resultData.missions.player2Mission.mid.missionNum = 1;
        resultData.missions.player2Mission.mid.isAchieved = true;
        resultData.missions.player2Mission.high.missionNum = 1;
        resultData.missions.player2Mission.high.isAchieved = false;

        resultData.missions.player3Mission.low.missionNum = 1;
        resultData.missions.player3Mission.low.isAchieved = true;
        resultData.missions.player3Mission.mid.missionNum = 1;
        resultData.missions.player3Mission.mid.isAchieved = true;
        resultData.missions.player3Mission.high.missionNum = 1;
        resultData.missions.player3Mission.high.isAchieved = false;

        resultData.missions.player4Mission.low.missionNum = 1;
        resultData.missions.player4Mission.low.isAchieved = true;
        resultData.missions.player4Mission.mid.missionNum = 1;
        resultData.missions.player4Mission.mid.isAchieved = true;
        resultData.missions.player4Mission.high.missionNum = 1;
        resultData.missions.player4Mission.high.isAchieved = false;

        firebaseManager.SaveResultData(resultData);
    }

    void UserDataSave()
    {
        FirebaseManager.userData.nickName = "Save1";
        firebaseManager.SaveUserData();
    }
    #endregion
}
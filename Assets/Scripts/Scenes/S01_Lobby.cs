using UnityEngine;
using Firebase.Auth;
using Photon.Realtime;
using TMPro;

public class S01_Lobby : Scenes
{
    #region Field
    /* Check Sign In*/
    FirebaseUser curUser;
    [SerializeField] TMP_Text ticket_count;
    [SerializeField] TMP_Text restTime;
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
        ticket_count.text = FirebaseManager.userData.itemData.ticket.ToString();
        restTime.text = FirebaseManager.restMinute + ":" + FirebaseManager.restSecond;
    }

    void OnDestroy()
    {
        restTime.text = "00:00";
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
    public void ShowHiText()
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
        // UserDataSave();
        // MinusTicket();
        GameDataSave();
    }

    void GameDataSave()
    {
        firebaseManager.InitializeGameData();
        for(int i = 0; i < 4; i++)
        {
            FirebaseManager.gameData.players[i].playerName = "player" + i;
            FirebaseManager.gameData.players[i].playerMission.low.missionNum = i;
            FirebaseManager.gameData.players[i].playerMission.mid.missionNum = i + 2;
            FirebaseManager.gameData.players[i].playerMission.high.missionNum = i + 4;
            for (int j = 0; j < 6; j++)
            {
                FirebaseManager.gameData.players[i].turnData[j].gameRoomNum = (j % 2 == 0);
                FirebaseManager.gameData.players[i].turnData[j].gold = j * 100;
                FirebaseManager.gameData.players[i].turnData[j].isProposer = (i % 2 == 0);
                FirebaseManager.gameData.players[i].turnData[j].isAchieved = ((i + j) % 2 == 0);
            }
        }
        firebaseManager.SaveGameData();
    }


    void UserDataSave()
    {
        FirebaseManager.userData.nickName = "Save1";
        firebaseManager.SaveUserData();
    }

    void MinusTicket()
    {
        if(firebaseManager.CanUseTicket()) firebaseManager.UseTicket();
    }
    #endregion
}
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Field
    public static bool IsPhotonReady = false;

    /* Singleton */
    private static PhotonManager _instance;

    /* Match Room */
    public static string StatusString, RoomString, NickNameString;
    readonly string gameVersion = "1";

    /* Gane Ready */
    bool[] playerReady = new bool[4];
    #endregion

    #region Singleton
    public static PhotonManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(typeof(PhotonManager).ToString());
                _instance = go.AddComponent<PhotonManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetPhoton();
    }

    #endregion

    #region Set Photon
    void SetPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;    // ���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting To Master Server...");
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        IsPhotonReady = true;
        Debug.Log("�������ӿϷ�");
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("�������");
    #endregion

    #region Set User ID
    public void SetUserID(string id)
    {
        PhotonNetwork.AuthValues.UserId = id;
        PhotonNetwork.LocalPlayer.NickName = NickNameString;
    }

    public void SignOutID()
    {
        PhotonNetwork.AuthValues.UserId = "";
    }
    #endregion

    #region Match Room
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => Debug.Log("�κ����ӿϷ�");

    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomString, new RoomOptions { MaxPlayers = 4, PublishUserId = true }, null);

    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomString);

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(RoomString, new RoomOptions { MaxPlayers = 4, PublishUserId = true }, null);

    public void LeaveRoom()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length != 1) 
        {
            photonView.TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
        }
        PhotonNetwork.LeaveRoom();
    }
    public override void OnCreatedRoom() => Debug.Log("�� ����� �Ϸ�");

    public override void OnJoinedRoom()
    {
        SpawnMyPlayerEverywhere();
        Debug.Log("�� ���� �Ϸ�");
    }
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("�� ����� ����");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("�� ���� ����");

    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("�� ���� ���� ����");

    public void RandomMatch() => PhotonNetwork.JoinRandomOrCreateRoom();
    #endregion

    #region Get Players Information In Room
    public string GetPlayerInformation(int num)
    {
        string player = PhotonNetwork.PlayerList[num].NickName.ToString();
        return player;
    }
    #endregion

    #region Spawn
    void SpawnMyPlayerEverywhere()
    {
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("instance ���� �Ϸ�");
    }
    #endregion

    #region Ready Game
    public void InitReady()
    {
        for (int i = 0; i < 4; i++)
        {
            playerReady[i] = false;
        }
    }

    public bool isReady(int playerNum)
    {
        return playerReady[playerNum - 1];
    }

    [PunRPC]
    public void ReadyOrNot(int playerNum, bool ready)
    {
        playerReady[playerNum - 1] = ready;
    }

    #endregion

    #region Syncronize and State
    [PunRPC] public string GetPlayerName(int playerNum) => FirebaseManager.gameData.players[playerNum].playerName;

    [PunRPC] public void SetTmd(TurnMatchData tmd) => FirebaseManager.turnMatchData = tmd;

    [PunRPC]
    public void ProposeGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, int proposeGold, int roomNum)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.Instance.SaveGameData();
    }

    [PunRPC]
    public void GetGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, bool isAchieved)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].isAchieved = isAchieved;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].isAchieved = isAchieved;

        FirebaseManager.Instance.SaveGameData();
    }

    [PunRPC]
    public int EndTurn(int roomNum)
    {
        return roomNum;
    }
    #endregion

    #region Information
    [ContextMenu("����")]
    public void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log($"Length : {PhotonNetwork.PlayerList.Length}");
            Debug.Log($"NickName : {PhotonNetwork.PlayerList[0].NickName}");

            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("�� ���� : " + PhotonNetwork.CountOfRooms);
            Debug.Log("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            Debug.Log("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion
}
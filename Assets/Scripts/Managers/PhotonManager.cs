using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Field
    public static bool IsPhotonReady = false;

    /* Singleton */
    private static PhotonManager _instance;

    /* Match Room */
    public static string StatusString, RoomString, NickNameString;
    private readonly string gameVersion = "1";

    /* Gane Ready */
    private bool[] playerReady = new bool[4];
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
    private void Awake()
    {
        SetPhoton();
    }

    #endregion

    #region Set Photon
    private void SetPhoton()
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
    }

    public void SetUserNickName()
    {
        PhotonNetwork.LocalPlayer.NickName = DatabaseManager.userData.nickName;
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
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length != 1)
        {
            photonView.TransferOwnership(PhotonNetwork.PlayerListOthers[0]);
        }
        DatabaseManager.enteredRoom = false;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnCreatedRoom() => Debug.Log("�� ����� �Ϸ�");

    public override void OnJoinedRoom()
    {
        DatabaseManager.enteredRoom = true;
        Debug.Log("�� ���� �Ϸ�");
        SpawnPlayerPrefab();    
    }
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("�� ����� ����");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("�� ���� ����");

    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("�� ���� ���� ����");

    public void RandomMatch() => PhotonNetwork.JoinRandomOrCreateRoom();
    #endregion

    #region Get Players Information In Room
    public string GetPlayerInformation(int num)
    {
        if(num >= PhotonNetwork.PlayerList.Length)
        {
            return "AI" + (num + 1 - PhotonNetwork.PlayerList.Length).ToString();
        }
        else
        {
            string player = PhotonNetwork.PlayerList[num].NickName.ToString();
            Debug.Log($"player {num} : {player}");
            return player;
        }
    }
    #endregion

    #region LoadScene
    public void LoadScene(int sceneNum)
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(sceneNum);
        SpawnPlayerPrefab();
    }
    #endregion

    #region Spawn
    public void SpawnPlayerPrefab()
    {
        int i = 0;
        while ((!DatabaseManager.enteredRoom) && i < 11)
        {
            Debug.Log("Delayed");
            i++;
            Thread.Sleep(1000);

        }
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
}
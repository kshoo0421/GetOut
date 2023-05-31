using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class B_PhotonManager : MonoBehaviourPunCallbacks
{
    #region Field
    /* Singleton */ 
    static B_PhotonManager instance;
    
    /* Photon Functions */
    public static string StatusString, RoomString, NickNameString;
    readonly string gameVersion = "1";
    #endregion

    #region Singleton
    B_PhotonManager() { }

    public static B_PhotonManager Instance { get { return instance; } }

    void SetSingleton()
    {
        if (instance == null) instance = GetComponent<B_PhotonManager>();
    }
    #endregion

    #region Monobehaviour
    void Awake()
    {
        SetSingleton();    
    }

    void Start()
    {
        SetPhoton();    
    }

    void Update() => StatusString = PhotonNetwork.NetworkClientState.ToString();

    #endregion

    #region Photon Functions
    void SetPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting To Master Server...");
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameString;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => Debug.Log("로비접속완료");

    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomString, new RoomOptions { MaxPlayers = 4 });

    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomString);

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(RoomString, new RoomOptions { MaxPlayers = 4 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => Debug.Log("방만들기완료");

    public override void OnJoinedRoom() => Debug.Log("방참가완료");

    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("방랜덤참가실패");

    [ContextMenu("정보")]
    public void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 개수 : " + PhotonNetwork.CountOfRooms);
            Debug.Log("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("로비에 있는지? : " + PhotonNetwork.InLobby);
            Debug.Log("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion
}

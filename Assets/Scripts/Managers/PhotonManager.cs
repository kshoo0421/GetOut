using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

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
        PhotonNetwork.AutomaticallySyncScene = true;    // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting To Master Server...");
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        IsPhotonReady = true;
        Debug.Log("서버접속완료");
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결끊김");
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

    public override void OnJoinedLobby() => Debug.Log("로비접속완료");

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
    public override void OnCreatedRoom() => Debug.Log("방 만들기 완료");

    public override void OnJoinedRoom()
    {
        DatabaseManager.enteredRoom = true;
        Debug.Log("방 참가 완료");
        SpawnPlayerPrefab();    
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방 만들기 실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방 참가 실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("방 랜덤 참가 실패");

    public void RandomMatch() => PhotonNetwork.JoinRandomOrCreateRoom();
    #endregion

    #region Spawn
    public void SpawnPlayerPrefab()
    {
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("instance 생성 완료");
    }
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
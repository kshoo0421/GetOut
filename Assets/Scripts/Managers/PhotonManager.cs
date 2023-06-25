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

    // void Update() => StatusString = PhotonNetwork.NetworkClientState.ToString();

    #endregion

    #region Set Photon
    void SetPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting To Master Server...");
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        IsPhotonReady = true;
        Debug.Log("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameString;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결끊김");
    #endregion

    #region Set User ID
    public void SetUserID(string id, string nickName)
    {
        PhotonNetwork.AuthValues.UserId = id;
        PhotonNetwork.LocalPlayer.NickName = nickName;
        Debug.Log($"Set User ID : {PhotonNetwork.AuthValues.UserId}");
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

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => Debug.Log("방 만들기 완료");

    public override void OnJoinedRoom() => Debug.Log("방 참가 완료");

    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방 만들기 실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방 참가 실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("방 랜덤 참가 실패");

    public void RandomMatch() => PhotonNetwork.JoinRandomOrCreateRoom();
    #endregion

    #region Get Players Information In Room
    public string GetPlayerInformation(int num)
    {
        string player = PhotonNetwork.PlayerList[num].NickName.ToString();
        Debug.Log($"playerList : {player}");
        return player;
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

    #region Instantiate
    /*
    모든 게임에서 플레이어에 하나 이상의 객체들을 생성 할 필요가 있습니다. 
    네트워크 게임에서 동기화 해야 하는 객체들은 특별한 작업 절차를 준수해야 합니다.
    */

    /* PhotonNetwork.Instantiate */   
    /*
    PUN에서는 PhotonNetwork.Instantiate 메소드에 시작위치, 회전정보 그리고 프리팹 이름을 전달하여 
    네트워크 객체를 자동적으로 스폰 할 수 있습니다.
    
    필요사항 : 실행 시에 로드하기 위하여 프리팹은 resources/ 폴더에 있어야 하며 PhotonView 컴포넌트가 있어야 합니다.
    */
    public void SpawnMyPlayerEverywhere()
    {
        PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0);
        //The last argument is an optional group number, feel free to ignore it for now.
    }

    /*
    새롭게 생성된 게임오브젝트를 설정 할 필요가 있는 경우에는 OnPhotonInstantiate(PhotonMessageInfo info)
    스크립트에서 기술 해주면 됩니다. 이 메소드는 누가 트리거를 했는지에 대한 정보를 가지고 호출됩니다.
    아래의 예는 플레이어의 TagObject로서 GameObject를 설정 하는 것을 보여줍니다. 
    */
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in PhotonPlayer.TagObject
        info.Sender.TagObject = this.GameObject();
    }


    /* 네트워크 객체의 생명주기 */
    /*
    PhotonNetwork.Instantiate 메소드에 의해서 생성된 게임 오브젝트들은 동일 룸내에 있으면 존재 합니다.
    만약 룸을 변경하면 유니티의 화면 전환처럼 네트워크 객체들은 변경된 룸으로 이동하지 않습니다.

    만약 클라이언트가 룸을 떠나게 되면 클라이언트 플레이어에게 소유 및 생성된 모든 게임오브젝트들은 사라지게 됩니다. 
    이러한 것이 게임 로직과 맞지 않는다면 이 단계는 건너뛰어도 됩니다. 
    게임에서 PhotonNetwork.autoCleanUpPlayerObjects 값을 false 로 설정하시기 바랍니다.

    선택적으로 마스터 클라이언트는 PhotonNetwork.InstantiateSceneObject() 메소드를 이용하여 룸의 생명주기를 가진 
    게임오브젝트들을 생성할 수 있습니다. 이 방식으로 생성된 게임오브젝트는 마스터 클라이언트가 아닌 룸과 연관되어 있습니다. 
    기본적으로 마스터 클라이언트가 이러한 게임오브젝트들을 제어하지만 photonView.TransferOwnership() 메소드를 통하여 
    제어권을 넘길 수 도 있습니다. 소유권 이전에 대한 데모를 확인 해 보시기 바랍니다. 
    */


    /* 네트워크 씬 객체들 */
    /*
    PhotonView는 씬내의 객체들 위에 위치시키는 것이 가장 좋습니다. 
    기본적으로 PhotonView들은 마스터 클라이언트에 의해 제어되며 "neutral" 객체를 통해 
    게임 방 관련 RPC(원격프로시져콜) 전송에 매우 유용 할 수 있습니다. 
    
    중요 : 네트워크 객체들을 가진 씬을 로드 할 때 게임방으로 들어가기 전에는 일부 PhotonView 값 들을 사용할 수 없습니다. 
    예를들어 : 게임방안에 들어가기 전에는 Awake() 메소드에서 isMine 으로 체크할 수 없습니다! 
    */


    /* 화면 전환 */
    /*
    일반적으로 유니티에서 씬을 로드했을 때 현재 계층 내의 모든 게임오브젝트들은 없어집니다. 
    이 제거 작업에는 네트워크 오브젝트들도 포함되며 상황에 따라 혼란스러울 수 있습니다. 
    
    예:메뉴 씬 에서 게임 방에 참가하고 다른 씬을 로드 합니다. 그 게임 방에 들어 갈 수 있고 게임방의 
    초기 메시지를 받을 수 도 있습니다. PUN은 네트워크 객체들을 생성하기 시작하지만 로직은 다른 씬을 
    로드하고 네트워크 객체들은 사라지게 됩니다. 
    
    씬을 로드할 때 이러한 문제를 없애기 위해서는 PhotonNetwork.automaticallySyncScene 값을 true 로 설정하고 
    PhotonNetwork.LoadLevel() 메소드를 사용하여 화면 전환을 할 수 있습니다. 
    */

    /* 수동 인스턴스생성 */
    /*
    네트워크를 통한 객체 생성을 resources 폴더에 의존하고 싶지 않다면 이 섹션 마지막에 나오는 예제처럼 수동으로 생성해야 합니다.
    
    수동 생성의 가장 큰 이유는 스트리밍 웹 플레이어에 어떤 것이 다운로드 되었는지에 대한 제어를 하려고 하는 것입니다. 
    유니티의 스트리밍과 resources 폴더의 상세 내용은 여기에서 보실 수 있습니다.

    RPC 전송으로 객체를 생성할 수 있습니다. 물론 원격 클라이언트에게 어떤 객체를 생성해야 할 지 알려주는 방법이 필요합니다. 
    GameObject의 참조 값을 보낼 수 없으므로 생성하려는 객체의 이름 또는 다른 무엇이 있어야 합니다.

    객체의 유형처럼 매우 중요한 것은 네트워크 id 입니다. PhotonView.viewID 는 네트워크 메시지를 올바른 
    게임오브젝트/스크립트에 전달하기 위한 키 입니다. 수동으로 스폰 하려면 PhotonNetwork.AllocateViewID() 
    메소드를 사용하여 새로운 viewID를 할당해야 하며 이 viewID를 통해 메시지를 전달합니다. 
    게임 방에 참가한 모든 사람들은 새로운 객체를 동일한 ID로 설정해야 합니다.

    인스턴스 생성을 위한 RPC는 버퍼화 되어야 합니다 : 나중에 접속한 클라이언트도 역시 스폰 명령을 받아야 합니다. 
    */

    public void SpawnPlayerEverywhere()
    {
        // You must be in a Room already

        // Manually allocate PhotonViewID
        int id1 = PhotonNetwork.AllocateViewID(true);

        PhotonView photonView = this.GetComponent<PhotonView>();
        photonView.RPC("SpawnOnNetwork", RpcTarget.AllBuffered, transform.position, transform.rotation, id1, PhotonNetwork.LocalPlayer);
    }

    public Transform playerPrefab; //set this in the inspector

    [PunRPC]
    public void SpawnOnNetwork(Vector3 pos, Quaternion rot, int id1, Player np)
    {
        Transform newPlayer = Instantiate(playerPrefab, pos, rot) as Transform;

        // Set player's PhotonView
        PhotonView[] nViews = newPlayer.GetComponentsInChildren<PhotonView>();
        nViews[0].ViewID = id1;
    }
    /*
    네트워크 객체들을 로드 하기 위하여 에셋 번들을 사용하고 싶으면 에셋 번들 로드 코드를 추가하고 
    예제에서 "playerPrefab"을 에셋 번들의 프리팹으로 교체 해 주면 됩니다. 
    */
    #endregion

    #region Syncronize and State
    #endregion


    #region Information
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
            Debug.Log($"Length : {PhotonNetwork.PlayerList.Length}");
            Debug.Log($"NickName : {PhotonNetwork.PlayerList[0].NickName}");

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
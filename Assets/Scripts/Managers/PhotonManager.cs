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
        Debug.Log("�������ӿϷ�");
        PhotonNetwork.LocalPlayer.NickName = NickNameString;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("�������");
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

    public override void OnJoinedLobby() => Debug.Log("�κ����ӿϷ�");

    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomString, new RoomOptions { MaxPlayers = 4, PublishUserId = true }, null);

    public void JoinRoom() => PhotonNetwork.JoinRoom(RoomString);

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(RoomString, new RoomOptions { MaxPlayers = 4, PublishUserId = true }, null);

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => Debug.Log("�� ����� �Ϸ�");

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("PlayerPrefab", Vector2.zero, Quaternion.identity);
        Debug.Log("�� ���� �� instance ���� �Ϸ�");
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
    ��� ���ӿ��� �÷��̾ �ϳ� �̻��� ��ü���� ���� �� �ʿ䰡 �ֽ��ϴ�. 
    ��Ʈ��ũ ���ӿ��� ����ȭ �ؾ� �ϴ� ��ü���� Ư���� �۾� ������ �ؼ��ؾ� �մϴ�.
    */

    /* PhotonNetwork.Instantiate */   
    /*
    PUN������ PhotonNetwork.Instantiate �޼ҵ忡 ������ġ, ȸ������ �׸��� ������ �̸��� �����Ͽ� 
    ��Ʈ��ũ ��ü�� �ڵ������� ���� �� �� �ֽ��ϴ�.
    
    �ʿ���� : ���� �ÿ� �ε��ϱ� ���Ͽ� �������� resources/ ������ �־�� �ϸ� PhotonView ������Ʈ�� �־�� �մϴ�.
    */
    public void SpawnMyPlayerEverywhere()
    {
        PhotonNetwork.Instantiate("MyPrefabName", new Vector3(0, 0, 0), Quaternion.identity, 0);
        //The last argument is an optional group number, feel free to ignore it for now.
    }

    /*
    ���Ӱ� ������ ���ӿ�����Ʈ�� ���� �� �ʿ䰡 �ִ� ��쿡�� OnPhotonInstantiate(PhotonMessageInfo info)
    ��ũ��Ʈ���� ��� ���ָ� �˴ϴ�. �� �޼ҵ�� ���� Ʈ���Ÿ� �ߴ����� ���� ������ ������ ȣ��˴ϴ�.
    �Ʒ��� ���� �÷��̾��� TagObject�μ� GameObject�� ���� �ϴ� ���� �����ݴϴ�. 
    */
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in PhotonPlayer.TagObject
        info.Sender.TagObject = this.GameObject();
    }


    /* ��Ʈ��ũ ��ü�� �����ֱ� */
    /*
    PhotonNetwork.Instantiate �޼ҵ忡 ���ؼ� ������ ���� ������Ʈ���� ���� �볻�� ������ ���� �մϴ�.
    ���� ���� �����ϸ� ����Ƽ�� ȭ�� ��ȯó�� ��Ʈ��ũ ��ü���� ����� ������ �̵����� �ʽ��ϴ�.

    ���� Ŭ���̾�Ʈ�� ���� ������ �Ǹ� Ŭ���̾�Ʈ �÷��̾�� ���� �� ������ ��� ���ӿ�����Ʈ���� ������� �˴ϴ�. 
    �̷��� ���� ���� ������ ���� �ʴ´ٸ� �� �ܰ�� �ǳʶپ �˴ϴ�. 
    ���ӿ��� PhotonNetwork.autoCleanUpPlayerObjects ���� false �� �����Ͻñ� �ٶ��ϴ�.

    ���������� ������ Ŭ���̾�Ʈ�� PhotonNetwork.InstantiateSceneObject() �޼ҵ带 �̿��Ͽ� ���� �����ֱ⸦ ���� 
    ���ӿ�����Ʈ���� ������ �� �ֽ��ϴ�. �� ������� ������ ���ӿ�����Ʈ�� ������ Ŭ���̾�Ʈ�� �ƴ� ��� �����Ǿ� �ֽ��ϴ�. 
    �⺻������ ������ Ŭ���̾�Ʈ�� �̷��� ���ӿ�����Ʈ���� ���������� photonView.TransferOwnership() �޼ҵ带 ���Ͽ� 
    ������� �ѱ� �� �� �ֽ��ϴ�. ������ ������ ���� ���� Ȯ�� �� ���ñ� �ٶ��ϴ�. 
    */


    /* ��Ʈ��ũ �� ��ü�� */
    /*
    PhotonView�� ������ ��ü�� ���� ��ġ��Ű�� ���� ���� �����ϴ�. 
    �⺻������ PhotonView���� ������ Ŭ���̾�Ʈ�� ���� ����Ǹ� "neutral" ��ü�� ���� 
    ���� �� ���� RPC(�������ν�����) ���ۿ� �ſ� ���� �� �� �ֽ��ϴ�. 
    
    �߿� : ��Ʈ��ũ ��ü���� ���� ���� �ε� �� �� ���ӹ����� ���� ������ �Ϻ� PhotonView �� ���� ����� �� �����ϴ�. 
    ������� : ���ӹ�ȿ� ���� ������ Awake() �޼ҵ忡�� isMine ���� üũ�� �� �����ϴ�! 
    */


    /* ȭ�� ��ȯ */
    /*
    �Ϲ������� ����Ƽ���� ���� �ε����� �� ���� ���� ���� ��� ���ӿ�����Ʈ���� �������ϴ�. 
    �� ���� �۾����� ��Ʈ��ũ ������Ʈ�鵵 ���ԵǸ� ��Ȳ�� ���� ȥ�������� �� �ֽ��ϴ�. 
    
    ��:�޴� �� ���� ���� �濡 �����ϰ� �ٸ� ���� �ε� �մϴ�. �� ���� �濡 ��� �� �� �ְ� ���ӹ��� 
    �ʱ� �޽����� ���� �� �� �ֽ��ϴ�. PUN�� ��Ʈ��ũ ��ü���� �����ϱ� ���������� ������ �ٸ� ���� 
    �ε��ϰ� ��Ʈ��ũ ��ü���� ������� �˴ϴ�. 
    
    ���� �ε��� �� �̷��� ������ ���ֱ� ���ؼ��� PhotonNetwork.automaticallySyncScene ���� true �� �����ϰ� 
    PhotonNetwork.LoadLevel() �޼ҵ带 ����Ͽ� ȭ�� ��ȯ�� �� �� �ֽ��ϴ�. 
    */

    /* ���� �ν��Ͻ����� */
    /*
    ��Ʈ��ũ�� ���� ��ü ������ resources ������ �����ϰ� ���� �ʴٸ� �� ���� �������� ������ ����ó�� �������� �����ؾ� �մϴ�.
    
    ���� ������ ���� ū ������ ��Ʈ���� �� �÷��̾ � ���� �ٿ�ε� �Ǿ������� ���� ��� �Ϸ��� �ϴ� ���Դϴ�. 
    ����Ƽ�� ��Ʈ���ְ� resources ������ �� ������ ���⿡�� ���� �� �ֽ��ϴ�.

    RPC �������� ��ü�� ������ �� �ֽ��ϴ�. ���� ���� Ŭ���̾�Ʈ���� � ��ü�� �����ؾ� �� �� �˷��ִ� ����� �ʿ��մϴ�. 
    GameObject�� ���� ���� ���� �� �����Ƿ� �����Ϸ��� ��ü�� �̸� �Ǵ� �ٸ� ������ �־�� �մϴ�.

    ��ü�� ����ó�� �ſ� �߿��� ���� ��Ʈ��ũ id �Դϴ�. PhotonView.viewID �� ��Ʈ��ũ �޽����� �ùٸ� 
    ���ӿ�����Ʈ/��ũ��Ʈ�� �����ϱ� ���� Ű �Դϴ�. �������� ���� �Ϸ��� PhotonNetwork.AllocateViewID() 
    �޼ҵ带 ����Ͽ� ���ο� viewID�� �Ҵ��ؾ� �ϸ� �� viewID�� ���� �޽����� �����մϴ�. 
    ���� �濡 ������ ��� ������� ���ο� ��ü�� ������ ID�� �����ؾ� �մϴ�.

    �ν��Ͻ� ������ ���� RPC�� ����ȭ �Ǿ�� �մϴ� : ���߿� ������ Ŭ���̾�Ʈ�� ���� ���� ����� �޾ƾ� �մϴ�. 
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
    ��Ʈ��ũ ��ü���� �ε� �ϱ� ���Ͽ� ���� ������ ����ϰ� ������ ���� ���� �ε� �ڵ带 �߰��ϰ� 
    �������� "playerPrefab"�� ���� ������ ���������� ��ü �� �ָ� �˴ϴ�. 
    */
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
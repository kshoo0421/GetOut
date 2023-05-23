using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;

public class S_LobbyManager : MonoBehaviourPunCallbacks
{
    #region �� ����
    private B_SceneChangeManager sceneChanger = B_SceneChangeManager.Instance;

    public void ChangeToScene(int sceneIndex)
    {
        sceneChanger.ChangetoScene(sceneIndex);
    }
    #endregion

    #region ���̾�̽� �� ���� Ȱ��
    private readonly string gameVersion = "1";
    private TotalGameManager totalGameManager;
    private FirebaseManager firebaseManager;

    public TMP_Text connectionInfoText;
    public Button joinButton;

    private void Awake()
    {
        totalGameManager = TotalGameManager.Instance;
        firebaseManager = FirebaseManager.Instance;
    }

    private void Start()
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

        if(PhotonNetwork.IsConnected)
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
    #endregion

    #region �׽�Ʈ
    public TMP_Text nameText;
    private FirebaseUser user;
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
}

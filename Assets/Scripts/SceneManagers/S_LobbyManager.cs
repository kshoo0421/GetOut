using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class S_LobbyManager : MonoBehaviourPunCallbacks
{
    #region 씬 변경
    private B_SceneChangeManager sceneChanger = new B_SceneChangeManager();

    public void ChangeToScene00()
    {
        sceneChanger.ChangetoScene(0);
    }

    public void ChangeToScene02()
    {
        sceneChanger.ChangetoScene(2);
    }

    public void ChangeToScene07()
    {
        sceneChanger.ChangetoScene(7);
    }

    public void ChangeToScene09()
    {
        sceneChanger.ChangetoScene(9);
    }

    public void ChangeToScene10()
    {
        sceneChanger.ChangetoScene(10);
    }

    public void ChangeToScene12()
    {
        sceneChanger.ChangetoScene(12);
    }

    public void ChangeToScene13()
    {
        sceneChanger.ChangetoScene(13);
    }
    #endregion

    #region 파이어베이스 및 포톤 활용
    private readonly string gameVersion = "1";

    public TMP_Text connectionInfoText;
    public Button joinButton;

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
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
}

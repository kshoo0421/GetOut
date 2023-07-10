using Firebase.Auth;
using TMPro;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class S02_WaitingRoom : Scenes
{
    #region Fields
    [SerializeField] private GameObject CustomMatchPanel;
    [SerializeField] private TMP_InputField customRoomNumField;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        photonManager.SetUserNickName();
    }

    private void Update()
    {
        ForUpdate();
    }
    #endregion

    #region Custom Match
    public void ToggleCustomMatchPanel()
    {
        if (CustomMatchPanel.activeSelf) CustomMatchPanel.SetActive(false);
        else CustomMatchPanel.SetActive(true);
    }

    public void CreateCustomRoom()
    {
        int randomNum = Random.Range(0, 999999999);
        PhotonManager.RoomString = randomNum.ToString();
        photonManager.CreateRoom();
        ChangeToScene(3);
    }

    public void JoinCustomRoom()
    {
        PhotonManager.RoomString = customRoomNumField.text;
        photonManager.JoinRoom();
        ChangeToScene(3);
    }
    #endregion

    #region RandomMatch
    public void RandomMatchBtn()
    {
        int randomNum = Random.Range(0, 999999999);
        PhotonManager.RoomString = randomNum.ToString();
        photonManager.RandomMatch();
        DatabaseManager.randomOrCustom = RandomOrCustom.Random;
        ChangeToScene(4);
    }
    #endregion
}
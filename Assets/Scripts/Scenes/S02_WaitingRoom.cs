using UnityEngine;
using TMPro;

public class S02_WaitingRoom : Scenes
{
    #region Fields
    [SerializeField] GameObject CustomMatchPanel;
    [SerializeField] TMP_InputField customRoomNumField;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
    }
    void Update()
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
        firebaseManager.InitDataForGame();
        ChangeToScene(3);
    }

    public void JoinCustomRoom()
    {
        PhotonManager.RoomString = customRoomNumField.text;
        photonManager.JoinRoom();
        firebaseManager.InitDataForGame();
        ChangeToScene(3);
    }
    #endregion

    #region RandomMatch
    public void RandomMatchBtn()
    {
        int randomNum = Random.Range(0, 999999999);
        PhotonManager.RoomString = randomNum.ToString();
        photonManager.RandomMatch();
        firebaseManager.InitDataForGame();
        ChangeToScene(4);
    }
    #endregion
}
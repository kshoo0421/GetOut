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
        Debug.Log("randomNum : " + randomNum);
        PhotonManager.RoomString = randomNum.ToString();
        Debug.Log("Room String : " + PhotonManager.RoomString);
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
        ChangeToScene(4);
    }

    #endregion
}
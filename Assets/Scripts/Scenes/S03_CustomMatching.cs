using UnityEngine;
using TMPro;
using Photon.Pun;

public class S03_CustomMatching : Scenes
{
    #region Field
    int curUserNum;

    [SerializeField] TMP_Text roomNumber; // room number
    [SerializeField] TMP_Text curState; // current state
    [SerializeField] TMP_Text[] playerNames;
    [SerializeField] TMP_Text[] playerOX;
    #endregion

    #region monobehaviour
    void Start()
    { 
        InitialSet();
        roomNumber.text = PhotonManager.RoomString;
    }
    void Update()
    {
        ForUpdate();
        UpdatePlayerNickName();
    }

    void OnDestroy()
    {
        ForOnDestroy();
    }

    #endregion

    #region Exit Custom Matching
    public void ExitCustomMatching()
    {
        photonManager.LeaveRoom();
        ChangeToScene(2);
    }
    #endregion

    #region Update String
    void UpdatePlayerNickName()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerNames[i].text = photonManager.GetPlayerInformation(i);
        }
    }

    #endregion
}
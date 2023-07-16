using TMPro;
using UnityEngine;

public class S04_RandomMatching : Scenes
{
    #region Field
    [SerializeField] private TMP_Text roomNumber; // room number
    [SerializeField] private TMP_Text curState; // current state
    [SerializeField] private TMP_Text[] PlayerNames;
    #endregion

    #region monobehaviour
    private void Start()
    {
        InitialSet();
        roomNumber.text = PhotonManager.RoomString;
    }

    private void Update()
    {
        ForUpdate();
    }
    #endregion

    #region Exit Random Matching
    public void ExitRandomMatching()
    {
        photonManager.LeaveRoom();
        ChangeToScene("WaitingRoom");
    }
    #endregion
}
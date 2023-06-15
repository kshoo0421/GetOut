using UnityEngine;
using TMPro;

public class S04_RandomMatching : Scenes
{
    #region Field
    [SerializeField] TMP_Text roomNumber; // room number
    [SerializeField] TMP_Text curState; // current state
    [SerializeField] TMP_Text[] PlayerNames;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        roomNumber.text = PhotonManager.RoomString;
    }
    #endregion

    #region Exit Random Matching
    public void ExitRandomMatching()
    {
        photonManager.LeaveRoom();
        ChangeToScene(2);
    }
    #endregion
}
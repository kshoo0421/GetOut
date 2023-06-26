using UnityEngine;
using TMPro;

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
}
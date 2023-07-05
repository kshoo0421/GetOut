using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSetPanel : MonoBehaviour
{
    #region Fields
    private GameData gd;    // game data
    private GamePlayer mp;  // my player

    [SerializeField] private Button[] RerollBtns;
    [SerializeField] private TMP_Text[] MissionInfoTMPs;
    [SerializeField] private TMP_Text[] MissionGoldTMPs;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        gd = DatabaseManager.gameData;
        mp = DatabaseManager.MyPlayer;
        InitMissionSet();
    }
    #endregion

    #region Functions
    private void InitMissionSet()
    {
        for (int i = 0; i < 3; i++)
        {
            RerollBtns[i].interactable = true;
        }
        // Mission Low, Mid, High Update
        MissionSetTextUpdate();
    }

    private void MissionSetTextUpdate() // need to change
    {
        MissionSupporter mi = new MissionSupporter();

        int[] missionNum = new int[3];
        missionNum[0] = (int)gd.playerMissionData[mp.playerNum].low.missionNum;
        missionNum[1] = (int)gd.playerMissionData[mp.playerNum].mid.missionNum;
        missionNum[2] = (int)gd.playerMissionData[mp.playerNum].high.missionNum;
        for (int i = 0; i < 3; i++)
        {
            MissionInfoTMPs[0].text = mi.GetMissionInfo((MissionLevel)i, missionNum[i]);
            MissionGoldTMPs[0].text = mi.GetMissionGold((MissionLevel)i, missionNum[i]).ToString() + "G";
        }
    }

    public void MissionReroll(int i)
    {
        // Mission Num Change
        switch (i)
        {
            case 0:
                mp.SetPlayerMission(MissionLevel.Low);
                break;
            case 1:
                mp.SetPlayerMission(MissionLevel.Mid);
                break;
            case 2:
                mp.SetPlayerMission(MissionLevel.High);
                break;
        }
        RerollBtns[i].interactable = false;
        mp.SavePlayerMissionData();
    }

    public void MissionFix()
    {
        for (int i = 0; i < 3; i++)
        {
            RerollBtns[i].interactable = false;
        }
    }
    #endregion

}

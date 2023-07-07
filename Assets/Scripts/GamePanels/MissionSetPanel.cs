using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSetPanel : MonoBehaviour
{
    #region Fields
    private GameData gd;    // game data
    private GamePlayer mp;  // my player
    MissionSupporter mi;

    [SerializeField] private Button FixButton;
    [SerializeField] private Button[] RerollBtns;
    [SerializeField] private TMP_Text[] MissionInfoTMPs;
    [SerializeField] private TMP_Text[] MissionGoldTMPs;
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        gd = DatabaseManager.gameData;
        mp = DatabaseManager.MyPlayer;
        mi =  new MissionSupporter();
        InitMissionSet();
        MissionSetTextUpdate();
    }
    #endregion

    #region Functions
    private void InitMissionSet()
    {
        for (int i = 0; i < 3; i++)
        {
            RerollBtns[i].interactable = true;
        }
    }

    private void MissionSetTextUpdate()
    {
        LowMissionTextUpdate();
        MidMissionTextUpdate();
        HighMissionTextUpdate();
    }

    private void LowMissionTextUpdate()
    {
        int missionNum = (int)gd.playerMissionData[mp.playerNum].low.missionNum;
        MissionInfoTMPs[0].text = mi.GetMissionInfo(MissionLevel.Low, missionNum);
        MissionGoldTMPs[0].text = mi.GetMissionGold(MissionLevel.Low, missionNum).ToString() + "G";
    }

    private void MidMissionTextUpdate()
    {
        int missionNum = (int)gd.playerMissionData[mp.playerNum].mid.missionNum;
        MissionInfoTMPs[1].text = mi.GetMissionInfo(MissionLevel.Mid, missionNum);
        MissionGoldTMPs[1].text = mi.GetMissionGold(MissionLevel.Mid, missionNum).ToString() + "G";
    }
    private void HighMissionTextUpdate()
    {
        int missionNum = (int)gd.playerMissionData[mp.playerNum].high.missionNum;
        MissionInfoTMPs[2].text = mi.GetMissionInfo(MissionLevel.High, missionNum);
        MissionGoldTMPs[2].text = mi.GetMissionGold(MissionLevel.High, missionNum).ToString() + "G";
    }

    public void MissionReroll(int i)
    {
        // Mission Num Change
        switch (i)
        {
            case 0:
                mp.SetPlayerMission(MissionLevel.Low);
                LowMissionTextUpdate();
                break;
            case 1:
                mp.SetPlayerMission(MissionLevel.Mid);
                MidMissionTextUpdate();
                break;
            case 2:
                mp.SetPlayerMission(MissionLevel.High);
                HighMissionTextUpdate();
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
        FixButton.interactable = false;
    }
    #endregion

}

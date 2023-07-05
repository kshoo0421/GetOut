using UnityEngine;

public class MissionSupporter : MonoBehaviour
{
    #region Singleton
    private static MissionSupporter _instance;
    public static MissionSupporter Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(typeof(MissionSupporter).ToString());
                _instance = go.AddComponent<MissionSupporter>();
                SetLowMissions();
                SetMidMissions();
                SetHighMissions();

            }
            return _instance;
        }
    }
    #endregion

    #region Init
    private static void SetLowMissions()
    {
        LowMission[0].gold = 50;
        LowMission[0].info = "low 0";

        LowMission[1].gold = 51;
        LowMission[1].info = "low 1";

        LowMission[2].gold = 52;
        LowMission[2].info = "low 2";

        LowMission[3].gold = 53;
        LowMission[3].info = "low 3";

        LowMission[4].gold = 54;
        LowMission[4].info = "low 4";

        LowMission[5].gold = 55;
        LowMission[5].info = "low 5";

        LowMission[6].gold = 56;
        LowMission[6].info = "low 6";

        LowMission[7].gold = 57;
        LowMission[7].info = "low 7";

        LowMission[8].gold = 58;
        LowMission[8].info = "low 8";

        LowMission[9].gold = 59;
        LowMission[9].info = "low 9";
    }
    private static void SetMidMissions()
    {
        MidMission[0].gold = 150;
        MidMission[0].info = "mid 0";

        MidMission[1].gold = 151;
        MidMission[1].info = "mid 1";

        MidMission[2].gold = 152;
        MidMission[2].info = "mid 2";

        MidMission[3].gold = 153;
        MidMission[3].info = "mid 3";

        MidMission[4].gold = 154;
        MidMission[4].info = "mid 4";

        MidMission[5].gold = 155;
        MidMission[5].info = "mid 5";

        MidMission[6].gold = 156;
        MidMission[6].info = "mid 6";

        MidMission[7].gold = 157;
        MidMission[7].info = "mid 7";

        MidMission[8].gold = 158;
        MidMission[8].info = "mid 8";

        MidMission[9].gold = 159;
        MidMission[9].info = "mid 9";
    }
    private static void SetHighMissions()
    {
        HighMission[0].gold = 250;
        HighMission[0].info = "High 0";

        HighMission[1].gold = 251;
        HighMission[1].info = "High 1";

        HighMission[2].gold = 252;
        HighMission[2].info = "High 2";

        HighMission[3].gold = 253;
        HighMission[3].info = "High 3";

        HighMission[4].gold = 254;
        HighMission[4].info = "High 4";

        HighMission[5].gold = 255;
        HighMission[5].info = "High 5";

        HighMission[6].gold = 256;
        HighMission[6].info = "High 6";

        HighMission[7].gold = 257;
        HighMission[7].info = "High 7";

        HighMission[8].gold = 258;
        HighMission[8].info = "High 8";

        HighMission[9].gold = 259;
        HighMission[9].info = "High 9";
    }

    #endregion

    #region Fields
    private static readonly Mission[] LowMission = new Mission[10];
    private static readonly Mission[] MidMission = new Mission[10];
    private static readonly Mission[] HighMission = new Mission[10];
    #endregion

    #region Functions
    public string GetMissionInfo(MissionLevel ml, long missionNum)
    {
        switch (ml)
        {
            case MissionLevel.Low:
                return LowMission[missionNum].info;

            case MissionLevel.Mid:
                return MidMission[missionNum].info;

            case MissionLevel.High:
                return HighMission[missionNum].info;

            default:
                Debug.LogError("MissionSupporter - GetMissionInfo Error");
                break;
        }
        return "null";
    }

    public int GetMissionGold(MissionLevel ml, long missionNum)
    {
        switch (ml)
        {
            case MissionLevel.Low:
                return LowMission[missionNum].gold;

            case MissionLevel.Mid:
                return MidMission[missionNum].gold;

            case MissionLevel.High:
                return HighMission[missionNum].gold;

            default:
                Debug.LogError("MissionSupporter - GetMissionGold Error");
                break;
        }
        return 0;
    }

    public bool CheckMissionDone(MissionLevel ml, long missionNum)
    {
        return false;
    }
    #endregion
}

internal struct Mission
{
    public int gold;
    public string info;
}
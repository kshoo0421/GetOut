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
            }
            return _instance;
        }
    }
    #endregion

    #region info strings / gold
    private static readonly string[] lowMissionInfo = new string[10] {
        "low 1",    // 1
        "low 2",    // 2
        "low 3",    // 3
        "low 4",    // 4
        "low 5",    // 5
        "low 6",    // 6
        "low 7",    // 7
        "low 8",    // 8
        "low 9",    // 9
        "low 10"     // 10
    };

    private static readonly int[] lowGoldsInfo = new int[10] {
        51, // 1
        52, // 2
        53, // 3
        54, // 4
        55, // 5
        56, // 6
        57, // 7
        58, // 8
        59, // 9
        60  // 10
    };

    private static readonly string[] midMissionInfo = new string[10] {
    "mid 1",    // 1
    "mid 2",    // 2
    "mid 3",    // 3
    "mid 4",    // 4
    "mid 5",    // 5
    "mid 6",    // 6
    "mid 7",    // 7
    "mid 8",    // 8
    "mid 9",    // 9
    "mid 10"     // 10
    };

    private static readonly int[] midGoldsInfo = new int[10] {
        151, // 1
        152, // 2
        153, // 3
        154, // 4
        155, // 5
        156, // 6
        157, // 7
        158, // 8
        159, // 9
        160  // 10
    };

    private static readonly string[] highMissionInfo = new string[10] {
    "high 1",    // 1
    "high 2",    // 2
    "high 3",    // 3
    "high 4",    // 4
    "high 5",    // 5
    "high 6",    // 6
    "high 7",    // 7
    "high 8",    // 8
    "high 9",    // 9
    "high 10"     // 10
    };

    private static readonly int[] highGoldsInfo = new int[10] {
        251, // 1
        252, // 2
        253, // 3
        254, // 4
        255, // 5
        256, // 6
        257, // 7
        258, // 8
        259, // 9
        260  // 10
    };
    #endregion

    #region Fields
    private static readonly Mission[] LowMission = new Mission[10] {
        new Mission(lowGoldsInfo[0], lowMissionInfo[0]),
        new Mission(lowGoldsInfo[1], lowMissionInfo[1]),
        new Mission(lowGoldsInfo[2], lowMissionInfo[2]),
        new Mission(lowGoldsInfo[3], lowMissionInfo[3]),
        new Mission(lowGoldsInfo[4], lowMissionInfo[4]),
        new Mission(lowGoldsInfo[5], lowMissionInfo[5]),
        new Mission(lowGoldsInfo[6], lowMissionInfo[6]),
        new Mission(lowGoldsInfo[7], lowMissionInfo[7]),
        new Mission(lowGoldsInfo[8], lowMissionInfo[8]),
        new Mission(lowGoldsInfo[9], lowMissionInfo[9]), 
    };

    private static readonly Mission[] MidMission = new Mission[10] {
        new Mission(midGoldsInfo[0], midMissionInfo[0]),
        new Mission(midGoldsInfo[1], midMissionInfo[1]),
        new Mission(midGoldsInfo[2], midMissionInfo[2]),
        new Mission(midGoldsInfo[3], midMissionInfo[3]),
        new Mission(midGoldsInfo[4], midMissionInfo[4]),
        new Mission(midGoldsInfo[5], midMissionInfo[5]),
        new Mission(midGoldsInfo[6], midMissionInfo[6]),
        new Mission(midGoldsInfo[7], midMissionInfo[7]),
        new Mission(midGoldsInfo[8], midMissionInfo[8]),
        new Mission(midGoldsInfo[9], midMissionInfo[9]),
    };

    private static readonly Mission[] HighMission = new Mission[10] {
        new Mission(highGoldsInfo[0], highMissionInfo[0]),
        new Mission(highGoldsInfo[1], highMissionInfo[1]),
        new Mission(highGoldsInfo[2], highMissionInfo[2]),
        new Mission(highGoldsInfo[3], highMissionInfo[3]),
        new Mission(highGoldsInfo[4], highMissionInfo[4]),
        new Mission(highGoldsInfo[5], highMissionInfo[5]),
        new Mission(highGoldsInfo[6], highMissionInfo[6]),
        new Mission(highGoldsInfo[7], highMissionInfo[7]),
        new Mission(highGoldsInfo[8], highMissionInfo[8]),
        new Mission(highGoldsInfo[9], highMissionInfo[9]),
    };
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

    public Mission(int gold, string info)
    {
        this.gold = gold;
        this.info = info;
    }
}
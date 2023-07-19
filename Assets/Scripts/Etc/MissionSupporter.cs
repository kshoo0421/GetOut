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
    private static readonly string[] enLowMissionInfo = new string[10] {
        "In my 'Get Phase', do 'Out' action 1 time or more",    // 1
        "In my 'Get Phase', do 'Get' action 2 times or more",    // 2
        "In my 'Suggest Phase', get more than 60G 1 time or more",    // 3
        "In my 'Suggest Phase', fail(opponent's 'Out' action) 1 time or more",    // 4
        "In my 'Suggest Phase', success(opponent's 'Get' action) 1 time or more",    // 5
        "In all games, fail(opponent's / my 'Out' action) 2 times or more",   // 6
        "In all games, success(opponent's / my 'Get' action) 3 times or more",   // 7
        "In my 'Suggest Phase', suggest '70G' to opponent 1 time or more",    // 8
        "In my 'Get Phase', get 30G or more 1 time or more",    // 9
        "In my 'Suggest Phase', suggest 30G or less 1 time or more"     // 10
    };

    private static readonly string[] koLowMissionInfo = new string[10] {
        "상대 제안 1회 이상 거절하기",    // 1
        "상대 제안 2회 이상 수락하기",    // 2
        "내 제안에서 60G 이상 획득하기",    // 3
        "상대가 내 제안을 1회 이상 거절하기",    // 4
        "상대가 내 제안을 1회 이상 수락하기",    // 5
        "모든 협상 중 2회 이상 거절하기",    // 6
        "모든 협상 중 3회 이상 수락하기",    // 7
        "내 제안 차례에서 70(나):30(상대) 제안하기",    // 8
        "상대 제안에서 30G 이상 획득하기",    // 9
        "내 제안 차례에서 40G 이하 제안하기"     // 10
    };

    private static readonly int[] lowGoldsInfo = new int[10] {
        40, // 1
        40, // 2
        40, // 3
        50, // 4
        50, // 5
        50, // 6
        50, // 7
        60, // 8
        60, // 9
        60  // 10
    };

    private static readonly string[] enMidMissionInfo = new string[10] {
    "In my 'Get Phase', do 'Out' action 2 times or more",    // 1
    "In my all 'Get Phase', do 'Get' action",    // 2
    "In my 'Suggest Phase', get more than 75G 1 time or more",    // 3
    "In my 'Suggest Phase', fail(opponent's 'Out' action) 2 times or more",    // 4
    "In my 'Suggest Phase', success(opponent's 'Get' action) 2 times or more",    // 5
    "In all game, fail(opponent's / my 'Out' action) 3 times or more",    // 6
    "In all game, success(opponent's / my 'Get' action) 4 times or more",    // 7
    "temp",    // 8
    "In my 'Suggest Phase', suggest 30G or less 1 time or more",    // 9
    "In my 'Get Phase', get 40G or more 1 time or more"     // 10
    };

    private static readonly string[] koMidMissionInfo = new string[10] {
    "상대 제안 2회 이상 거절하기",    // 1
    "상대 제안 모두 수락하기",    // 2
    "내 제안에서 75G 이상 획득하기",    // 3
    "상대가 내 제안을 2회 이상 거절하기",    // 4
    "상대가 내 제안을 2회 이상 수락하기",    // 5
    "모든 협상 중 3회 이상 거절하기",    // 6
    "모든 협상 중 4회 이상 수락하기",    // 7
    "temp",    // 8
    "내 제안 차례에서 30G 이하 제안하기",    // 9
    "상대 제안에서 40G 이상 획득하기"     // 10
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

    private static readonly string[] enHighMissionInfo = new string[10] {
    "In my all 'Get Phase', do 'Out' action",    // 1
    "In my 'Suggest Phase', get '80G' or more 1 time or more",    // 2
    "In my 'Get Phase', get '70G' or more 1 time or more",    // 3
    "In my all 'Suggest Phase', opponent do 'Get' action",    // 4
    "In my all 'Suggest Phase', opponent do 'Out' action",    // 5
    "In all game, occur 'Out' action 4 times or more",    // 6
    "In all game, occur 'Get' action 5 times or more",    // 7
    "In my 'Get Phase', do 'Get' - 'Out' - 'Out' action in order",    // 8
    "In my 'Get Phase', do 'Out' - 'Get' - 'Out' action in order",    // 9
    "In my 'Get Phase', do 'Out' - 'Out' - 'Get' action in order",    // 10
    };

    private static readonly string[] koHighMissionInfo = new string[10] {
    "상대 제안 모두 거절하기",    // 1
    "내 제안 차례에서 80G 이상 획득하기",    // 2
    "상대 제안에서 70G 이상 획득하기",    // 3
    "상대가 내 제안을 모두 수락하기",    // 4
    "상대가 내 제안을 모두 거절하기",    // 5
    "모든 협상 중 4회 이상 거절하기",    // 6
    "모든 협상 중 5회 이상 수락하기",    // 7
    "수락을 ‘수락’ - ‘거절’ - ‘거절’ 순으로 하기",    // 8
    "수락을 ‘거절’ - ‘수락’ - ‘거절’ 순으로 하기",    // 9
    "수락을 ‘거절’ - ‘거절’ - ‘수락’ 순으로 하기"     // 10
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
        new Mission(lowGoldsInfo[0], enLowMissionInfo[0], koLowMissionInfo[0]),
        new Mission(lowGoldsInfo[1], enLowMissionInfo[1], koLowMissionInfo[1]),
        new Mission(lowGoldsInfo[2], enLowMissionInfo[2], koLowMissionInfo[2]),
        new Mission(lowGoldsInfo[3], enLowMissionInfo[3], koLowMissionInfo[3]),
        new Mission(lowGoldsInfo[4], enLowMissionInfo[4], koLowMissionInfo[4]),
        new Mission(lowGoldsInfo[5], enLowMissionInfo[5], koLowMissionInfo[5]),
        new Mission(lowGoldsInfo[6], enLowMissionInfo[6], koLowMissionInfo[6]),
        new Mission(lowGoldsInfo[7], enLowMissionInfo[7], koLowMissionInfo[7]),
        new Mission(lowGoldsInfo[8], enLowMissionInfo[8], koLowMissionInfo[8]),
        new Mission(lowGoldsInfo[9], enLowMissionInfo[9], koLowMissionInfo[9])
    };

    private static readonly Mission[] MidMission = new Mission[10] {
        new Mission(midGoldsInfo[0], enMidMissionInfo[0], koMidMissionInfo[0]),
        new Mission(midGoldsInfo[1], enMidMissionInfo[1], koMidMissionInfo[1]),
        new Mission(midGoldsInfo[2], enMidMissionInfo[2], koMidMissionInfo[2]),
        new Mission(midGoldsInfo[3], enMidMissionInfo[3], koMidMissionInfo[3]),
        new Mission(midGoldsInfo[4], enMidMissionInfo[4], koMidMissionInfo[4]),
        new Mission(midGoldsInfo[5], enMidMissionInfo[5], koMidMissionInfo[5]),
        new Mission(midGoldsInfo[6], enMidMissionInfo[6], koMidMissionInfo[6]),
        new Mission(midGoldsInfo[7], enMidMissionInfo[7], koMidMissionInfo[7]),
        new Mission(midGoldsInfo[8], enMidMissionInfo[8], koMidMissionInfo[8]),
        new Mission(midGoldsInfo[9], enMidMissionInfo[9], koMidMissionInfo[9])
    };

    private static readonly Mission[] HighMission = new Mission[10] {
        new Mission(highGoldsInfo[0], enHighMissionInfo[0], koHighMissionInfo[0]),
        new Mission(highGoldsInfo[1], enHighMissionInfo[1], koHighMissionInfo[1]),
        new Mission(highGoldsInfo[2], enHighMissionInfo[2], koHighMissionInfo[2]),
        new Mission(highGoldsInfo[3], enHighMissionInfo[3], koHighMissionInfo[3]),
        new Mission(highGoldsInfo[4], enHighMissionInfo[4], koHighMissionInfo[4]),
        new Mission(highGoldsInfo[5], enHighMissionInfo[5], koHighMissionInfo[5]),
        new Mission(highGoldsInfo[6], enHighMissionInfo[6], koHighMissionInfo[6]),
        new Mission(highGoldsInfo[7], enHighMissionInfo[7], koHighMissionInfo[7]),
        new Mission(highGoldsInfo[8], enHighMissionInfo[8], koHighMissionInfo[8]),
        new Mission(highGoldsInfo[9], enHighMissionInfo[9], koHighMissionInfo[9])
    };
    #endregion

    #region Functions
    public string GetMissionInfo(MissionLevel ml, long missionNum)
    {
        int locale = OptionManager.curLocale;
        switch (ml)
        {
            case MissionLevel.Low:
                switch(locale)
                {
                    case 0:
                        return LowMission[missionNum].enInfo;
                    case 1:
                        return LowMission[missionNum].koInfo;
                    default:
                        return LowMission[missionNum].enInfo;
                }

            case MissionLevel.Mid:
                switch (locale)
                {
                    case 0:
                        return MidMission[missionNum].enInfo;
                    case 1:
                        return MidMission[missionNum].koInfo;
                    default:
                        return MidMission[missionNum].enInfo;
                }

            case MissionLevel.High:
                switch (locale)
                {
                    case 0:
                        return HighMission[missionNum].enInfo;
                    case 1:
                        return HighMission[missionNum].koInfo;
                    default:
                        return HighMission[missionNum].enInfo;
                }

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
    public string enInfo;
    public string koInfo;

    public Mission(int gold, string enInfo, string koInfo)
    {
        this.gold = gold;
        this.enInfo = enInfo;
        this.koInfo = koInfo;
    }
}
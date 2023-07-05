public class MissionInfo
{
    private Mission[] LowMission = new Mission[10];
    private Mission[] MidMission = new Mission[10];
    private Mission[] HighMission = new Mission[10];

    private void OnEnable()
    {
        SetLowMissions();
        SetMidMissions();
        SetHighMissions();
    }

    public void SetLowMissions()
    {
        LowMission[0].gold = 50;
        LowMission[0].info = "low 0";

        LowMission[1].gold = 50;
        LowMission[1].info = "low 1";

        LowMission[2].gold = 50;
        LowMission[2].info = "low 2";

        LowMission[3].gold = 50;
        LowMission[3].info = "low 3";

        LowMission[4].gold = 50;
        LowMission[4].info = "low 4";

        LowMission[5].gold = 50;
        LowMission[5].info = "low 5";

        LowMission[6].gold = 50;
        LowMission[6].info = "low 6";

        LowMission[7].gold = 50;
        LowMission[7].info = "low 7";

        LowMission[8].gold = 50;
        LowMission[8].info = "low 8";

        LowMission[9].gold = 50;
        LowMission[9].info = "low 9";
    }
    public void SetMidMissions()
    {
        MidMission[0].gold = 150;
        MidMission[0].info = "mid 0";

        MidMission[1].gold = 150;
        MidMission[1].info = "mid 1";

        MidMission[2].gold = 150;
        MidMission[2].info = "mid 2";

        MidMission[3].gold = 150;
        MidMission[3].info = "mid 3";

        MidMission[4].gold = 150;
        MidMission[4].info = "mid 4";

        MidMission[5].gold = 150;
        MidMission[5].info = "mid 5";

        MidMission[6].gold = 150;
        MidMission[6].info = "mid 6";

        MidMission[7].gold = 150;
        MidMission[7].info = "mid 7";

        MidMission[8].gold = 150;
        MidMission[8].info = "mid 8";

        MidMission[9].gold = 150;
        MidMission[9].info = "mid 9";
    }
    public void SetHighMissions()
    {
        HighMission[0].gold = 250;
        HighMission[0].info = "High 0";

        HighMission[1].gold = 250;
        HighMission[1].info = "High 1";

        HighMission[2].gold = 250;
        HighMission[2].info = "High 2";

        HighMission[3].gold = 250;
        HighMission[3].info = "High 3";

        HighMission[4].gold = 250;
        HighMission[4].info = "High 4";

        HighMission[5].gold = 250;
        HighMission[5].info = "High 5";

        HighMission[6].gold = 250;
        HighMission[6].info = "High 6";

        HighMission[7].gold = 250;
        HighMission[7].info = "High 7";

        HighMission[8].gold = 250;
        HighMission[8].info = "High 8";

        HighMission[9].gold = 250;
        HighMission[9].info = "High 9";
    }

    public string GetMissionInfo(MissionLevel ml, int missionNum)
    {
        switch (ml)
        {
            case (MissionLevel)0:
                return LowMission[missionNum].info;

            case (MissionLevel)1:
                return MidMission[missionNum].info;

            case (MissionLevel)2:
                return HighMission[missionNum].info;

            default:
                break;
        }
        return "null";
    }

    public int GetMissionGold(MissionLevel ml, int missionNum)
    {
        switch (ml)
        {
            case (MissionLevel)0:
                return LowMission[missionNum].gold;

            case (MissionLevel)1:
                return MidMission[missionNum].gold;

            case (MissionLevel)2:
                return HighMission[missionNum].gold;

            default:
                break;
        }
        return 0;
    }


    public bool CheckMissionDone(MissionLevel ml, int missionNum)
    {
        return false;
    }

}

internal struct Mission
{
    public int gold;
    public string info;
}
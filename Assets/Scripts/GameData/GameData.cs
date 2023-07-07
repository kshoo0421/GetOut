using JetBrains.Annotations;
using System;
using UnityEngine.Playables;

#region Data
[Serializable]
public struct GameData
{
    public long gameIndex;
    public bool[] playerReady;
    public string[] playerId;
    public PlayerMissionData[] playerMissionData;
    public TurnData[] turnData;

    public GameData(int index)
    {
        gameIndex = index;
        playerReady = new bool[4] { false, false, false, false };
        playerId = new string[4] { "AI1", "AI2", "AI3", "AI4" };
        playerMissionData = new PlayerMissionData[4];
        for (int i = 0; i < 4; i++)
        {
            playerMissionData[i] = new PlayerMissionData(0);
        }
        turnData = new TurnData[6];
        for(int i = 0; i < 6; i++)
        {
            turnData[i] = new TurnData(0);
        }
    }
}

[Serializable]
public struct TurnData
{
    public long[] matchWith;
    public long[] gold;
    public bool[] success;
    public bool[] isSuggestor;

    public TurnData(int i)
    {
        matchWith = new long[4] { -1, -1, -1, -1 };
        gold = new long[4] { 0, 0, 0, 0 };
        success = new bool[4] { false, false, false, false };
        isSuggestor = new bool[4] { false, false, false, false };
    }
}

[Serializable]
public struct PlayerMissionData  // 개인 미션 데이터
{
    public MissionData low; // 난이도 하
    public MissionData mid; // 난이도 중
    public MissionData high;    // 난이도 상

    public PlayerMissionData(int i)
    {
        low = new MissionData(0, false);
        mid = new MissionData(0, false);
        high = new MissionData(0, false);
    }
}

[Serializable]
public struct MissionData  // 미션 데이터
{
    public long missionNum; // 미션 번호
    public bool isAchieved; // 성취 여부

    public MissionData(long i, bool b)
    {
        missionNum = i;
        isAchieved = b;
    }
}
#endregion
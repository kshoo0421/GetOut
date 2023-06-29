using System;

public enum MissionLevel { Low, Mid, High };

#region Data
[Serializable] public struct GameData
{
    public long gameIndex;
    public long curTurn;
    public bool[] playerReady;
    public Players[] players;  // length : 4
}

[Serializable] public struct Players
{
    public string playerName;
    public TurnData[] turnData; // length : 6
    public PlayerMissionData playerMission;
    public long finalRank;   // 등수
}

[Serializable] public struct TurnData
{
    public bool gameRoomNum;    // true : room1, false : room2
    public bool isProposer;
    public long gold;
    public bool isAchieved;
}

[Serializable]
public struct PlayerMissionData  // 개인 미션 데이터
{
    public MissionData low; // 난이도 하
    public MissionData mid; // 난이도 중
    public MissionData high;    // 난이도 상
}

[Serializable]
public struct MissionData  // 미션 데이터
{
    public long missionNum; // 미션 번호
    public bool isAchieved; // 성취 여부
}
#endregion
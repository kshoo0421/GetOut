using System;

public enum MissionLevel { Low = 0, Mid = 1, High = 2 };

public enum GamePhase
{
    Default, InitGame, SetMission, LoadingPhase,
    SuggestPhase, GetPhase, WaitingGetPhase, WaitingSuggestPhase, ResultPhase
}

#region Data
[Serializable]
public struct GameData
{
    public long gameIndex;
    public long curTurn;
    public bool isProposerTurn;
    public bool[] playerReady;
    public string[] playerId;
    public PlayerMissionData[] playerMissionData;
    public TurnData[] turnData;

}

[Serializable]
public struct TurnData
{
    public long[] matchWith;
    public long[] gold;
    public bool[] success;
    public bool[] isProposer;
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
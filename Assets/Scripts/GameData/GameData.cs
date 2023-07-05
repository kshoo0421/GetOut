using System;

#region Data
[Serializable]
public struct GameData
{
    public long gameIndex;
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
    public bool[] isSuggestor;
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
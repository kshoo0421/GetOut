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
public struct PlayerMissionData  // ���� �̼� ������
{
    public MissionData low; // ���̵� ��
    public MissionData mid; // ���̵� ��
    public MissionData high;    // ���̵� ��
}

[Serializable]
public struct MissionData  // �̼� ������
{
    public long missionNum; // �̼� ��ȣ
    public bool isAchieved; // ���� ����
}
#endregion
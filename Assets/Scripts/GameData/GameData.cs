using System;

public enum MissionLevel { Low, Mid, High };

#region Data
[Serializable] public struct GameData
{
    public long gameIndex;
    public PlayersD[] players;  // length : 4
}

[Serializable] public struct PlayersD
{
    public string playerName;
    public TurnData[] turnData; // length : 6
    public PlayerMissionData playerMission;
    public long finalRank;   // ���
}

[Serializable] public struct TurnData
{
    public bool gameRoomNum;    // true : room1, false : room2
    public bool isProposer;
    public long gold;
    public bool isAchieved;
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
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
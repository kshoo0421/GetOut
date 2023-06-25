using System;

public enum MissionLevel { Low, Mid, High };

//#region old Data
//[Serializable]
//public struct ResultData
//{
//    // �⺻ ���� ���� - �ش� �� ���� ����
//    public long gameIndex;

//    // ������ ����
//    public Players players;

//    // �Ϻ� ����
//    public Turns turns;

//    // �̼� ����
//    public Missions missions;

//    // �� �÷��̾� ����
//    public TotalScores totalScores;

//    // ���� ���
//    public FinalResult finalResult;
//}

//[Serializable]
//public struct Players
//{
//    public PlayerData player1;
//    public PlayerData player2;
//    public PlayerData player3;
//    public PlayerData player4;
//}

//[Serializable]
//public struct PlayerData // ���� ����
//{
//    public string playerName;
//    public string playerId;
//    public long curGold;
//}

//[Serializable]
//public struct Turns
//{
//    public OneTurnData turn1;
//    public OneTurnData turn2;
//    public OneTurnData turn3;
//    public OneTurnData turn4;
//    public OneTurnData turn5;
//    public OneTurnData turn6;
//}

//[Serializable]
//public struct OneTurnData  // �� �Ͽ� �� �÷��̾��� ������
//{
//    public PlayerTurnData player1;
//    public PlayerTurnData player2;
//    public PlayerTurnData player3;
//    public PlayerTurnData player4;
//}

//[Serializable]
//public struct PlayerTurnData  // �� �Ͽ� �� �÷��̾��� ������
//{
//    public long value; // ���� �ݾ�
//    public bool isGet; // �ŷ� ���� ����
//}


//[Serializable]
//public struct Missions
//{
//    public PlayerMissionData player1Mission;
//    public PlayerMissionData player2Mission;
//    public PlayerMissionData player3Mission;
//    public PlayerMissionData player4Mission;
//}


//[Serializable]
//public struct TotalScores // ���� ����
//{
//    public long player1;
//    public long player2;
//    public long player3;
//    public long player4;
//}

//[Serializable]
//public struct FinalResult // ���� ���
//{
//    public long first;
//    public long second;
//    public long third;
//    public long fourth;
//}
//#endregion

// new Data(test)

#region new Data(test)
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
#endregion

#region common data
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
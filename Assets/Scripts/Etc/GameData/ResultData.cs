using System;

[Serializable]
public struct ResultData  
{
    // �⺻ ���� ���� - �ش� �� ���� ����
    public string GAME_KEY;

    // ������ ����
    PlayerData player1;
    PlayerData player2;
    PlayerData player3;
    PlayerData player4;


    // �Ϻ� ����
    OneTurnData turn1;
    OneTurnData turn2;
    OneTurnData turn3;
    OneTurnData turn4;
    OneTurnData turn5;
    OneTurnData turn6;

    // �̼� ����
    PlayerMissionData player1Mission;
    PlayerMissionData player2Mission;
    PlayerMissionData player3Mission;
    PlayerMissionData player4Mission;

    // �� �÷��̾� ����
    TotalScores totalScores;

    // ���� ���
    FinalResult finalResult;
}
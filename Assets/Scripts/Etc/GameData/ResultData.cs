using System;

[Serializable]
public struct ResultData  
{
    // �⺻ ���� ���� - �ش� �� ���� ����
    public string GAME_KEY;

    // ������ ����
    public PlayerData player1;
    public PlayerData player2;
    public PlayerData player3;
    public PlayerData player4;


    // �Ϻ� ����
    public OneTurnData turn1;
    public OneTurnData turn2;
    public OneTurnData turn3;
    public OneTurnData turn4;
    public OneTurnData turn5;
    public OneTurnData turn6;

    // �̼� ����
    public PlayerMissionData player1Mission;
    public PlayerMissionData player2Mission;
    public PlayerMissionData player3Mission;
    public PlayerMissionData player4Mission;

    // �� �÷��̾� ����
    public TotalScores totalScores;

    // ���� ���
    public FinalResult finalResult;
}
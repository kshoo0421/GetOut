using System;

[Serializable]
public struct ResultData
{
    // �⺻ ���� ���� - �ش� �� ���� ����
    public long gameIndex;

    // ������ ����
    public Players players;

    // �Ϻ� ����
    public Turns turns;

    // �̼� ����
    public Missions missions;

    // �� �÷��̾� ����
    public TotalScores totalScores;

    // ���� ���
    public FinalResult finalResult;
}
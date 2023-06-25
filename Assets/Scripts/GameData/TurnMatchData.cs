using System;

[Serializable]
public struct TurnMatchData  // ���� ����
{
    public OpponentData t1OpponentData;
    public OpponentData t2OpponentData;
    public OpponentData t3OpponentData;
    public OpponentData t4OpponentData;
    public OpponentData t5OpponentData;
    public OpponentData t6OpponentData;
}

[Serializable]
public struct OpponentData  // 1player ���� ���
{
    public long opponent;    // ��밡 �����ΰ�
    public bool isProposer; // ������ vs ������
}
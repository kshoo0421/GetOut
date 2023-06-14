using System;

[Serializable]
public struct TurnMatchData  // ���� ����
{
    OpponentData t1OpponentData;
    OpponentData t2OpponentData;
    OpponentData t3OpponentData;
    OpponentData t4OpponentData;
    OpponentData t5OpponentData;
    OpponentData t6OpponentData;
}

[Serializable]
public struct OpponentData  // 1player ���� ���
{
    public long opponent;    // ��밡 �����ΰ�
    public bool isProposer; // ������ vs ������
}
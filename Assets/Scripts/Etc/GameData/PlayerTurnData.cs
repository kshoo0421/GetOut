using System;

[Serializable]
public struct PlayerTurnData  // 한 턴에 한 플레이어의 데이터
{
    public long value; // 제시 금액
    public bool isGet; // 거래 성사 여부
}
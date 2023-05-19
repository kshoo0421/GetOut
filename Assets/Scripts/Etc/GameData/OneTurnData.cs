using System;

[Serializable]
public struct OneTurnData  // 한 턴에 한 플레이어의 데이터
{
    PlayerTurnData player1;
    PlayerTurnData player2;
    PlayerTurnData player3;
    PlayerTurnData player4;
}
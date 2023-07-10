public enum MissionLevel { Low = 0, Mid = 1, High = 2 };

public enum GamePhase
{
    Default, InitGame, SetMission, StartTurn, 
    Suggest, WaitingSuggest, Get, WaitingGet, TurnResult, FinalResult
}

public enum RandomOrCustom { Default, Custom, Random };

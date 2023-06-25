using System.Linq;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    #region Field
    TurnMatchData tmd;
    #endregion


    #region Set Game
    void SetGame()
    {
        tmd = new TurnMatchData();
        // FirebaseManager.resultData = new ResultData();
        FirebaseManager.gameData= new GameData();
        SetOpponent();
    }

    void SetOpponent()
    {
        int[] arr = { 1, 2, 3, 4, 5, 6 };
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();

        tmd.t1OpponentData.opponent = getOpponent(arr[0]);
        tmd.t1OpponentData.isProposer = getIsProposer(arr[0]);

        tmd.t2OpponentData.opponent = getOpponent(arr[1]);
        tmd.t2OpponentData.isProposer = getIsProposer(arr[1]);

        tmd.t3OpponentData.opponent = getOpponent(arr[2]);
        tmd.t3OpponentData.isProposer = getIsProposer(arr[2]);

        tmd.t4OpponentData.opponent = getOpponent(arr[3]);
        tmd.t4OpponentData.isProposer = getIsProposer(arr[3]);

        tmd.t5OpponentData.opponent = getOpponent(arr[4]);
        tmd.t5OpponentData.isProposer = getIsProposer(arr[4]);

        tmd.t6OpponentData.opponent = getOpponent(arr[5]);
        tmd.t6OpponentData.isProposer = getIsProposer(arr[5]);

    }

    public void SetPlayerMission(int playerNumber, MissionLevel missionLevel)
    {

    }

    void SetMissionLevel(MissionLevel missionLevel)
    {
        switch (missionLevel)
        {
            case MissionLevel.Low:

                break;
            case MissionLevel.Mid:
                break;
            case MissionLevel.High:
                break;
            default: break;
        }
    }




    int getOpponent(int i) => ((i + 1) / 2);

    bool getIsProposer(int i) => (i % 2 == 0);


    #endregion

}

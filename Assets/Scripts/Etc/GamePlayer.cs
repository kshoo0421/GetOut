using Photon.Pun;
using System.Linq;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    #region Field
    public int playerNum;
    public bool isMasterClient;

    public PhotonView view;
    private GameData gd;
    private GamePhase gp;
    private DatabaseManager databaseManager;
    private int playerLength;

    #endregion

    #region MonoBehabiour
    private void OnEnable()
    {
        databaseManager =  DatabaseManager.Instance;
        
        if (view.IsMine == true)
        {
            DatabaseManager.MyPlayer = this;
            playerNum = (view.ViewID / 1000) - 1;
        }
        gd = DatabaseManager.gameData;
        gp = DatabaseManager.gamePhase;
        CheckIsMasterClient();
        if(isMasterClient)
        {
            playerLength = PhotonNetwork.PlayerList.Length;
            InitAi();
        }
    }
    #endregion

    #region Check Master Client
    private void CheckIsMasterClient()
    {
        if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
        {
            isMasterClient = true;
        }
    }
    #endregion

    #region Random / Custom Room
    public void ToggleGameReady()   // ready or not to start game
    {
        view.RPC("RPC_ToggleGameReady", RpcTarget.All, playerNum);
    }
    [PunRPC] private void RPC_ToggleGameReady(int playerNum) => gd.playerReady[playerNum] = !gd.playerReady[playerNum];
    #endregion

    #region In Game Common Function
    // Synchronize Turn Data
    public void SynchronizeTurn(int curTurn) // Syncronize turn data
    {
        view.RPC("RPC_SynchronizeTurn", RpcTarget.All, curTurn);
    }
    [PunRPC] private void RPC_SynchronizeTurn(int curTurn) => DatabaseManager.curTurn = curTurn;

    // Synchronize Left Time
    public void SynchronizeLeftTime(string leftTime)
    {
        view.RPC("RPC_SynchronizeLeftTime", RpcTarget.All, leftTime);
    }
    [PunRPC] private void RPC_SynchronizeLeftTime(string leftTime) => DatabaseManager.leftTime = leftTime;


    // Synchronize GameData
    public void SaveGameData()
    {
        view.RPC("RPC_SaveGameData", RpcTarget.All);
    }
    [PunRPC] private void RPC_SaveGameData() => DatabaseManager.Instance.SaveGameData();

    // Save Player Mission Data
    public void SavePlayerMissionData()
    {
        view.RPC("RPC_SavePlayerMissionData", RpcTarget.All);

        if(isMasterClient)  // for AI
        {
            SavePlayerMissionDataForAi();
        }
    }
    [PunRPC]
    private void RPC_SavePlayerMissionData()
    {
        long low = DatabaseManager.gameData.playerMissionData[playerNum].low.missionNum;
        long mid = DatabaseManager.gameData.playerMissionData[playerNum].mid.missionNum;
        long high = DatabaseManager.gameData.playerMissionData[playerNum].high.missionNum;
        DatabaseManager.Instance.SavePlayerMissionData(playerNum, new PlayerMissionData(low, mid, high));
    }

    private void SavePlayerMissionDataForAi()
    {
        for (int i = playerLength; i < 4; i++)
        {
            view.RPC("RPC_SavePlayerMissionDataForAi", RpcTarget.All, i, (long)aiLowMission[i], (long)aiMidMission[i], (long)aiHighMission[i]);
        }
    }
    [PunRPC]
    private void RPC_SavePlayerMissionDataForAi(int aiNum, long low, long mid, long high)
    {
        DatabaseManager.Instance.SavePlayerMissionData(aiNum, new PlayerMissionData(low, mid, high));
    }
    #endregion

    #region Suggest Phase
    // when the suggest phase start, Set Suggest Phase
    public void SetSuggestPhase(int suggest1, int suggest2)
    {
        view.RPC("RPC_SetSuggestPhase", RpcTarget.All, suggest1, suggest2);
    }
    [PunRPC] private void RPC_SetSuggestPhase(int suggest1, int suggest2)
    {
        if (playerNum == suggest1 || playerNum == suggest2)
        {
            DatabaseManager.gamePhase = GamePhase.Suggest;
        }
        else
        {
            DatabaseManager.gamePhase = GamePhase.WaitingSuggest;
        }
    }

    public void SuggestGold(int suggest1, int suggest2)
    {
        view.RPC("RPC_SuggestGold", RpcTarget.All, suggest1, suggest2);
        SuggestGoldForAi(suggest1, suggest2);
    }

    [PunRPC] private void RPC_SuggestGold(int suggest1, int suggest2)
    {
        if (playerNum == suggest1 || playerNum == suggest2)
        {
            int curTurn = DatabaseManager.curTurn;
            int goldAmount = DatabaseManager.goldAmount;
            int otherPlayerNum = (int)gd.turnData[curTurn].matchWith[playerNum];

            DatabaseManager.gameData.turnData[curTurn].gold[playerNum] = goldAmount;
            DatabaseManager.gameData.turnData[curTurn].isSuggestor[playerNum] = true;

            DatabaseManager.gameData.turnData[curTurn].gold[otherPlayerNum] = 100 - goldAmount;
            DatabaseManager.gameData.turnData[curTurn].isSuggestor[otherPlayerNum] = false;

            DatabaseManager.Instance.SaveTurnData(curTurn);
        }
    }

    public void SuggestGoldForAi(int suggest1, int suggest2)
    {
        for (int i = playerLength; i < 4; i++)
        {
            if (i == suggest1 || i == suggest2)
            {
                int otherPlayerNum = (int)gd.turnData[DatabaseManager.curTurn].matchWith[i];
                int goldAmount = AiSuggest(i);
                view.RPC("RPC_SuggestGoldForAi", RpcTarget.All, i, otherPlayerNum, goldAmount);
            }        
        }
    }

    [PunRPC] private void RPC_SuggestGoldForAi(int aiNum, int otherPlayerNum, int goldAmount)
    {
        int curTurn = DatabaseManager.curTurn;
        
        DatabaseManager.gameData.turnData[curTurn].gold[aiNum] = goldAmount;
        DatabaseManager.gameData.turnData[curTurn].isSuggestor[aiNum] = true;

        DatabaseManager.gameData.turnData[curTurn].gold[otherPlayerNum] = 100 - goldAmount;
        DatabaseManager.gameData.turnData[curTurn].isSuggestor[otherPlayerNum] = false;

        DatabaseManager.Instance.SaveTurnData(curTurn);
    }
    #endregion

    #region Get Phase
    // When get phase started, set get phase
    public void SetGetPhase(int getter1, int getter2)
    {
        view.RPC("RPC_SetGetPhase", RpcTarget.All, getter1, getter2);
    }

    [PunRPC]
    private void RPC_SetGetPhase(int getter1, int getter2)
    {
        if (playerNum == getter1 || playerNum == getter2)
        {
            DatabaseManager.gamePhase = GamePhase.Get;
        }
        else
        {
            DatabaseManager.gamePhase = GamePhase.WaitingGet;
        }
    }

    // In th end, select get or out the gold
    public void GetOutGold(int getter1, int getter2)
    {
        view.RPC("RPC_GetOutGold", RpcTarget.All, playerNum, getter1, getter2);
        GetOutGoldForAi(getter1, getter2);
    }

    [PunRPC] private void RPC_GetOutGold(int playerNum, int getter1, int getter2)
    {
        if(playerNum == getter1 || playerNum == getter2) // if player is getter
        {
            int curTurn = DatabaseManager.curTurn;
            int otherPlayerNum = (int)DatabaseManager.gameData.turnData[curTurn].matchWith[playerNum];
            bool isGet = DatabaseManager.isGet;
            DatabaseManager.gameData.turnData[curTurn].success[playerNum] = isGet;
            DatabaseManager.gameData.turnData[curTurn].success[otherPlayerNum] = isGet;
            DatabaseManager.Instance.SaveTurnData(curTurn);
        }
    }

    private void GetOutGoldForAi(int getter1, int getter2)
    {
        int curTurn = DatabaseManager.curTurn;
        for (int i = playerLength; i < 4; i++)
        {
            if (i == getter1 || i == getter2) // if player is getter
            {
                int otherPlayerNum = (int)DatabaseManager.gameData.turnData[curTurn].matchWith[i];
                bool isGet = AiGet(i, (int)DatabaseManager.gameData.turnData[curTurn].gold[i]);
                view.RPC("RPC_GetOutGoldForAi", RpcTarget.All, i, otherPlayerNum, isGet);
            }
        }
    }
    [PunRPC]
    private void RPC_GetOutGoldForAi(int aiNum, int otherPlayerNum, bool isGet)
    {
        int curTurn = DatabaseManager.curTurn;
        DatabaseManager.gameData.turnData[curTurn].success[aiNum] = isGet;
        DatabaseManager.gameData.turnData[curTurn].success[otherPlayerNum] = isGet;
        DatabaseManager.Instance.SaveTurnData(curTurn);
    }
    #endregion

    #region other Phase
    public void SetGamePhase(GamePhase gamePhase)   // common phase
    {
        view.RPC("RPC_SetGamePhase", RpcTarget.All, gamePhase);
    }
    [PunRPC] private void RPC_SetGamePhase(GamePhase gamePhase) => DatabaseManager.gamePhase = gamePhase;
    #endregion

    #region Set Game
    public void SetGame()
    {
        if (isMasterClient)
        {
            TurnMatchData tmd = new TurnMatchData();
            SetOpponent(ref tmd);
            InitGdWithTmd(ref tmd);
            InitPlayerMissions();
            SetGamePhase(GamePhase.SetMission);
        }
        databaseManager.SaveGameData();
    }

    private void SetOpponent(ref TurnMatchData tmd)
    {
        tmd.turn = new Turn[6];
        Turn[] turns = InitTurns();
        System.Random random = new System.Random();
        for (int i = 0; i < 3; i++)
        {
            if (random.Next() % 2 == 0)
            {
                tmd.turn[i * 2] = turns[i * 4];
                tmd.turn[(i * 2) + 1] = turns[(i * 4) + 1];
            }
            else
            {
                tmd.turn[i * 2] = turns[(i * 4) + 2];
                tmd.turn[(i * 2) + 1] = turns[(i * 4) + 3];
            }
        }
        tmd.turn = tmd.turn.OrderBy(x => random.Next()).ToArray();
    }

    private Turn[] InitTurns()
    {
        Turn[] turns = new Turn[12];
        turns[0].R1S = 0;
        turns[0].R1G = 1;
        turns[0].R2S = 2;
        turns[0].R2G = 3;

        turns[1].R1S = 1;
        turns[1].R1G = 0;
        turns[1].R2S = 3;
        turns[1].R2G = 2;
        //----------------------------
        turns[2].R1S = 0;
        turns[2].R1G = 1;
        turns[2].R2S = 3;
        turns[2].R2G = 2;

        turns[3].R1S = 1;
        turns[3].R1G = 0;
        turns[3].R2S = 2;
        turns[3].R2G = 3;
        //----------------------------
        turns[4].R1S = 0;
        turns[4].R1G = 2;
        turns[4].R2S = 1;
        turns[4].R2G = 3;

        turns[5].R1S = 2;
        turns[5].R1G = 0;
        turns[5].R2S = 3;
        turns[5].R2G = 1;
        //----------------------------
        turns[6].R1S = 0;
        turns[6].R1G = 2;
        turns[6].R2S = 3;
        turns[6].R2G = 1;

        turns[7].R1S = 2;
        turns[7].R1G = 0;
        turns[7].R2S = 1;
        turns[7].R2G = 3;
        //----------------------------
        turns[8].R1S = 0;
        turns[8].R1G = 3;
        turns[8].R2S = 1;
        turns[8].R2G = 2;

        turns[9].R1S = 3;
        turns[9].R1G = 0;
        turns[9].R2S = 2;
        turns[9].R2G = 1;
        //----------------------------
        turns[10].R1S = 0;
        turns[10].R1G = 3;
        turns[10].R2S = 2;
        turns[10].R2G = 1;

        turns[11].R1S = 3;
        turns[11].R1G = 0;
        turns[11].R2S = 1;
        turns[11].R2G = 2;

        return turns;
    }

    private void InitGdWithTmd(ref TurnMatchData tmd)
    {
        DatabaseManager.curTurn = -1;
        for (int i = 0; i < 6; i++)
        {
            gd.turnData[i].matchWith[(int)tmd.turn[i].R1S] = tmd.turn[i].R1G;
            gd.turnData[i].matchWith[(int)tmd.turn[i].R1G] = tmd.turn[i].R1S;
            gd.turnData[i].matchWith[(int)tmd.turn[i].R2S] = tmd.turn[i].R2G;
            gd.turnData[i].matchWith[(int)tmd.turn[i].R2G] = tmd.turn[i].R2S;

            gd.turnData[i].isSuggestor[(int)tmd.turn[i].R1S] = true;
            gd.turnData[i].isSuggestor[(int)tmd.turn[i].R1G] = false;
            gd.turnData[i].isSuggestor[(int)tmd.turn[i].R2S] = true;
            gd.turnData[i].isSuggestor[(int)tmd.turn[i].R2G] = false;
        }
    }

    private void InitPlayerMissions()
    {
        SetPlayerMission(MissionLevel.Low);
        SetPlayerMission(MissionLevel.Mid);
        SetPlayerMission(MissionLevel.High);
    }

    public void SetPlayerMission(MissionLevel missionLevel)
    {
        System.Random random = new System.Random();
        long tmp = (long)random.Next(1, 10);
        switch (missionLevel)
        {
            case MissionLevel.Low:
                while (gd.playerMissionData[playerNum].low.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                gd.playerMissionData[playerNum].low.missionNum = tmp;
                break;
            case MissionLevel.Mid:
                while (gd.playerMissionData[playerNum].mid.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                gd.playerMissionData[playerNum].mid.missionNum = tmp;
                break;
            case MissionLevel.High:
                while (gd.playerMissionData[playerNum].high.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                gd.playerMissionData[playerNum].high.missionNum = tmp;
                break;
            default: break;
        }
    }
    #endregion

    #region AI
    private int[] aiLowMission = new int[4];
    private int[] aiMidMission = new int[4];
    private int[] aiHighMission = new int[4];
    private int[] aiSuggestMin = new int[4];
    private int[] aiSuggestMax= new int[4];
    private int[] aiGetMin = new int[4];
    private int[] aiGetMax = new int[4];

    private void InitAi()
    {
        int tmp = 0;
        for(int i = 0; i < 4; i++)
        {
            aiLowMission[i] = Random.Range(0, 10);
            aiMidMission[i] = Random.Range(0, 10);
            aiHighMission[i] = Random.Range(0, 10);
            
            tmp = Random.Range(50, 101);
            aiSuggestMin[i] = Random.Range(50, tmp);
            aiSuggestMax[i] = Random.Range(aiSuggestMin[i], 101);

            tmp = Random.Range(0, 50);
            aiGetMin[i] = Random.Range(0, tmp);
            aiGetMax[i] = Random.Range(aiGetMin[i], 101);
        }
    }

    private int AiSuggest(int playerNum)
    {
        return Random.Range(aiSuggestMin[playerNum], aiSuggestMax[playerNum] + 1);
    }

    private bool AiGet(int playerNum, int goldAmount)
    {
        if (goldAmount >= aiGetMin[playerNum] && goldAmount <= aiGetMax[playerNum])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    private void RPC_SetUserNickName(string nickName, int playerNum)
    {
        DatabaseManager.gameData.playerId[playerNum] = nickName;
    }


    #endregion
}

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
    DatabaseManager databaseManager;
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
    }

    private void OnDestroy()
    {
        Debug.Log($"player {playerNum} : Destroyed");
    }
    #endregion

    private void CheckIsMasterClient()
    {
        if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
        {
            isMasterClient = true;
        }
    }

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

    // Synchronize GameData
    public void SynchronizeGameData()
    {
        view.RPC("RPC_SynchronizeGameData", RpcTarget.All, playerNum);
    }
    [PunRPC] private void RPC_SynchronizeGameData() => DatabaseManager.Instance.UpdateGameData();

    // Save Player Mission Data
    public void SavePlayerMissionData()
    {
        view.RPC("RPC_SavePlayerMissionData", RpcTarget.All);
    }
    [PunRPC] private void RPC_SavePlayerMissionData(int playerNum) => DatabaseManager.Instance.SavePlayerMissionData(playerNum);
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
            gp = GamePhase.SuggestPhase;
        }
        else
        {
            gp = GamePhase.WaitingSuggestPhase;
        }
    }

    // in the end, suggest gold
    public void SuggestGold()
    {
        int otherPlayerNum = (int)gd.turnData[DatabaseManager.curTurn].matchWith[playerNum];
        int goldAmount = DatabaseManager.goldAmount;
        view.RPC("RPC_SuggestGold", RpcTarget.All, playerNum, otherPlayerNum, goldAmount);
    }

    [PunRPC] private void RPC_SuggestGold(int playerNum, int otherPlayerNum, int proposeGold)
    {
        int curTurn = DatabaseManager.curTurn;
        gd.turnData[curTurn].gold[playerNum] = proposeGold;
        gd.turnData[curTurn].isSuggestor[playerNum] = true;

        gd.turnData[curTurn].gold[otherPlayerNum] = proposeGold;
        gd.turnData[curTurn].isSuggestor[otherPlayerNum] = false;

        DatabaseManager.Instance.SaveTurnData(curTurn);
    }
    #endregion

    #region Get Phase
    // When get phase started, set get phase
    public void SetGetPhase(int get1, int get2)
    {
        view.RPC("RPC_SetGetPhase", RpcTarget.All, get1, get2);
    }

    [PunRPC]
    private void RPC_SetGetPhase(int get1, int get2)
    {
        if (playerNum == get1 || playerNum == get2)
        {
            gp = GamePhase.GetPhase;
        }
        else
        {
            gp = GamePhase.WaitingGetPhase;
        }
    }

    // In th end, select get or out the gold
    public void GetOutGold()
    {
        int otherPlayerNum = (int)gd.turnData[DatabaseManager.curTurn].matchWith[playerNum];
        bool isGet = DatabaseManager.isGet;
        view.RPC("RPC_GetOutGold", RpcTarget.All, playerNum, otherPlayerNum, isGet);
    }
    [PunRPC] private void RPC_GetOutGold(int playerNum, int otherPlayerNum, bool isGet)
    {
        int curTurn = DatabaseManager.curTurn;
        gd.turnData[curTurn].success[playerNum] = isGet;
        gd.turnData[curTurn].success[otherPlayerNum] = isGet;

        DatabaseManager.Instance.SaveGameData();
    }
    #endregion

    #region other Phase
    public void SetGamePhase(GamePhase gamePhase)   // common phase
    {
        view.RPC("RPC_SetGamePhase", RpcTarget.All, gd);
    }
    [PunRPC] private void RPC_SetGamePhase(GamePhase gamePhase) => DatabaseManager.gamePhase = gamePhase;
    #endregion

    #region Set Game
    public void SetGame()
    {
        if (isMasterClient)
        {
            gd = new GameData();
            TurnMatchData tmd = new TurnMatchData();
            SetOpponent(ref tmd);
            InitGameData(ref tmd);
            InitPlayerMissions();
            SetGamePhase(GamePhase.SetMission);
        }
    }

    private void SetOpponent(ref TurnMatchData tmd)
    {
        Debug.Log("Set Opponent");
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

    private void InitGameData(ref TurnMatchData tmd)
    {
        GameData gd = DatabaseManager.gameData;
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

        int length = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < 4; i++)
        {
            if (i < length)
            {
                gd.playerId[i] = PhotonNetwork.PlayerList[i].UserId;
            }
            else
            {
                gd.playerId[i] = "AI" + (i - length);
            }
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
        long tmp = random.Next(1, 10);
        switch (missionLevel)
        {
            case MissionLevel.Low:
                Debug.Log($"playerNum : {playerNum}");//, gd.playerMissionData[playerNum] : {gd.playerMissionData[playerNum]}");
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

}

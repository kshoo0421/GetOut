using UnityEngine;
using Unity.VisualScripting;

using Photon.Pun;
using System.Linq;
using JetBrains.Annotations;

public class GamePlayer : MonoBehaviour, IPunInstantiateMagicCallback
{
    #region Field
    public int playerNum;
    public bool isPlayerCaptain;

    public PhotonView view;
    #endregion

    #region MonoBehabiour
    void OnEnable()
    {
        view = GetComponent<PhotonView>(); 
        if(view.IsMine == true)
        {
            DatabaseManager.MyPlayer = this;
            playerNum = (view.ViewID / 1000) - 1;
        }
    }
    #endregion

    #region IPunInstantiateMagicCallback
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.GameObject();
    }
    #endregion

    #region PunRPC
    [PunRPC] 
    void RPC_ReadyForGame(int playerNum)
    {
        DatabaseManager.gameData.playerReady[playerNum] = !DatabaseManager.gameData.playerReady[playerNum];
    }

    [PunRPC] void RPC_SetGameData(GameData gameData) => DatabaseManager.gameData = gameData;

    [PunRPC] void RPC_SetGamePhase(GamePhase gamePhase) => DatabaseManager.gamePhase = gamePhase;

    [PunRPC] 
    void RPC_SetSuggestPhase(int suggest1, int suggest2)
    {
        if (playerNum == suggest1 || playerNum == suggest2)
        {
            DatabaseManager.gamePhase = GamePhase.SuggestPhase;
        }
        else
        {
            DatabaseManager.gamePhase = GamePhase.WaitingSuggestPhase;
        }
    }
    
    [PunRPC] 
    void RPC_SetGetPhase(int get1, int get2)
    {
        if (playerNum == get1 || playerNum == get2)
        {
            DatabaseManager.gamePhase = GamePhase.GetPhase;
        }
        else
        {
            DatabaseManager.gamePhase = GamePhase.WaitingGetPhase;
        }
    }

    [PunRPC] 
    void RPC_SavePlayerMissionData(int playerNum) => DatabaseManager.Instance.SavePlayerMissionData(playerNum);

    [PunRPC] 
    void RPC_SuggestGold(int playerNum, int otherPlayerNum, int proposeGold)
    {
        GameData gameData = DatabaseManager.gameData;

        gameData.turnData[gameData.curTurn].gold[playerNum] = proposeGold;
        gameData.turnData[gameData.curTurn].isProposer[playerNum] = true;

        gameData.turnData[gameData.curTurn].gold[otherPlayerNum] = proposeGold;
        gameData.turnData[gameData.curTurn].isProposer[otherPlayerNum] = false;

        DatabaseManager.Instance.SaveTurnData((int)gameData.curTurn);
    }

    [PunRPC] 
    void RPC_GetGold(int playerNum, int otherPlayerNum, int turnNum, bool isAchieved)
    {
        GameData gameData = DatabaseManager.gameData;

        gameData.turnData[gameData.curTurn].success[playerNum] = isAchieved;
        gameData.turnData[gameData.curTurn].success[otherPlayerNum] = isAchieved;

        DatabaseManager.Instance.SaveGameData();
    }
    #endregion

    #region For RPC Functions
    public void TogglePlayerReady()
    {
        view.RPC("RPC_ReadyForGame", RpcTarget.All, playerNum);
    }

    public void SetGameData()
    {
        view.RPC("RPC_SetGameData", RpcTarget.All, DatabaseManager.gameData);
    }

    public void SetGamePhase(GamePhase gamePhase)
    {
        view.RPC("RPC_SetGamePhase", RpcTarget.All, DatabaseManager.gamePhase);
    }

    public void SetGetPhase(int get1, int get2)
    {
        view.RPC("RPC_SetGetPhase", RpcTarget.All, get1, get2);
    }


    public void SetSuggestPhase(int suggest1, int suggest2)
    {
        view.RPC("RPC_SetSuggestPhase", RpcTarget.All, suggest1, suggest2);
    }

    public void SavePlayerMissionData()
    {
        view.RPC("RPC_SavePlayerMissionData", RpcTarget.All, playerNum);
    }

    public void SuggestGold(int otherNum, int proposeGold)
    {
        view.RPC("RPC_SuggestGold", RpcTarget.All, playerNum, otherNum, proposeGold);
    }

    public void GetOutGold(int otherPlayerNum, bool isAchieved)
    {
        view.RPC("RPC_GetGold", RpcTarget.All, playerNum, otherPlayerNum, DatabaseManager.gameData.curTurn, isAchieved);
    }
    #endregion

    #region Set Game
    public void SetGame()
    {
        isPlayerCaptain = (view.ViewID == 1001) ? true : false;
        if (isPlayerCaptain)
        {
            DatabaseManager.gameData = new GameData();
            TurnMatchData tmd;
            SetOpponent(out tmd);
            InitGameData(ref tmd);
            InitPlayerMissions();
            SetGamePhase(GamePhase.SetMission);
        }
    }

    void SetOpponent(out TurnMatchData tmd)
    {
        tmd.turn = new Turn[6];
        Turn[] turns = InitTurns();
        System.Random random = new System.Random();
        for (int i = 0; i < 3; i++)
        {
            if (random.Next() % 2 == 0)
            {
                tmd.turn[i * 2] = turns[i * 4];
                tmd.turn[i * 2 + 1] = turns[i * 4 + 1];

            }
            else
            {
                tmd.turn[i * 2] = turns[i * 4 + 2];
                tmd.turn[i * 2 + 1] = turns[i * 4 + 3];
            }
        }
        tmd.turn = tmd.turn.OrderBy(x => random.Next()).ToArray();

        for(int i = 0; i < 6; i++)
        {
            Debug.Log($"tmd.turn[{i}].R1S : {tmd.turn[i].R1S}");
            Debug.Log($"tmd.turn[{i}].R1G : {tmd.turn[i].R1G}");
            Debug.Log($"tmd.turn[{i}].R2S : {tmd.turn[i].R2S}");
            Debug.Log($"tmd.turn[{i}].R2G : {tmd.turn[i].R2G}");
            Debug.Log($"-------------------------------------");
        }
    }

    Turn[] InitTurns()
    {
        Turn[] turns = new Turn[12];
        turns[0].R1S= 0;
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

    void InitGameData(ref TurnMatchData tmd)
    {
        GameData gd = DatabaseManager.gameData;
        gd.curTurn = -1;
        gd.isProposerTurn = false;
        for(int i =0; i < 6; i++)
        {
            Debug.Log($"i : {i}, tmd.turn[i].R1P : {tmd.turn[i].R1S}, tmd.turn[i].R1G : {tmd.turn[i].R1G}");
            Debug.Log($"gd.turnData[i].matchWith[tmd.turn[i].R1S] : {gd.turnData[i].matchWith[tmd.turn[i].R1S]}");
            gd.turnData[i].matchWith[tmd.turn[i].R1S] = tmd.turn[i].R1G;
            gd.turnData[i].matchWith[tmd.turn[i].R1G] = tmd.turn[i].R1S;
            gd.turnData[i].matchWith[tmd.turn[i].R2S] = tmd.turn[i].R2G;
            gd.turnData[i].matchWith[tmd.turn[i].R2G] = tmd.turn[i].R2S;

            gd.turnData[i].isProposer[tmd.turn[i].R1S] = true;
            gd.turnData[i].isProposer[tmd.turn[i].R1G] = false;
            gd.turnData[i].isProposer[tmd.turn[i].R2S] = true;
            gd.turnData[i].isProposer[tmd.turn[i].R2G] = false;
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

    void InitPlayerMissions()
    {
        SetPlayerMission(MissionLevel.Low);
        SetPlayerMission(MissionLevel.Mid);
        SetPlayerMission(MissionLevel.High);
    }

    public void SetPlayerMission(MissionLevel missionLevel)
    {
        System.Random random = new System.Random();
        int tmp = random.Next(1, 10);
        switch (missionLevel)
        {
            case MissionLevel.Low:
                while (DatabaseManager.gameData.playerMissionData[playerNum].low.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                DatabaseManager.gameData.playerMissionData[playerNum].low.missionNum = tmp;
                break;
            case MissionLevel.Mid:
                while (DatabaseManager.gameData.playerMissionData[playerNum].mid.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                DatabaseManager.gameData.playerMissionData[playerNum].mid.missionNum = tmp;
                break;
            case MissionLevel.High:
                while (DatabaseManager.gameData.playerMissionData[playerNum].high.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                DatabaseManager.gameData.playerMissionData[playerNum].high.missionNum = tmp;
                break;
            default: break;
        }
    }
    #endregion

}

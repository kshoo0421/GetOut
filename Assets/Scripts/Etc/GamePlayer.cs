using Photon.Pun;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class GamePlayer : MonoBehaviour, IPunInstantiateMagicCallback
{
    #region Field
    public static string[] playerNames;

    int playerNum;
    bool isProposer;
    public PhotonView view;
    #endregion

    #region MonoBehabiour
    void OnEnable()
    {
        playerNames = new string[PhotonNetwork.PlayerList.Length];
        
        view = GetComponent<PhotonView>(); 
        if(view.IsMine == true)
        {
            FirebaseManager.MyPlayer = this;
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

    #region For RPC Functions
    public void TogglePlayerReady()
    {
        view.RPC("ReadyForGame", RpcTarget.All, playerNum);
    }

    public void SetTmdIfP1()
    {
        if(view.ViewID == 1001)
        {
            view.RPC("SetTmd", RpcTarget.All, FirebaseManager.turnMatchData);
        }
    }

    public void GetOutGold(int otherPlayerNum, bool isAchieved)
    {
        view.RPC("GetGoldInTurn", RpcTarget.All, playerNum, otherPlayerNum, FirebaseManager.gameData.curTurn, isAchieved);
    }
    #endregion

    #region PunRPC
    [PunRPC] void SetTmd(TurnMatchData tmd) => FirebaseManager.turnMatchData = tmd;

    [PunRPC] void ReadyForGame(int playerNum)
    {
        FirebaseManager.gameData.playerReady[playerNum] = !FirebaseManager.gameData.playerReady[playerNum];
    }

    [PunRPC] void ProposeGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, int proposeGold, int roomNum)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.Instance.SaveGameData();
    }

    [PunRPC] void GetGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, bool isAchieved)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].isAchieved = isAchieved;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].isAchieved = isAchieved;

        FirebaseManager.Instance.SaveGameData();
    }
    #endregion
}

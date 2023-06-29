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

    #endregion

    #region MonoBehabiour
    void OnEnable()
    {
        playerNames = new string[PhotonNetwork.PlayerList.Length];
        SetPlayerNum();
    }

    void OnDisable()
    {

    }
    #endregion

    #region IPunInstantiateMagicCallback
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.Sender.TagObject = this.GameObject();
    }
    #endregion

    #region Set Player Num
    void SetPlayerNum()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] == FirebaseManager.userData.id)
            {
                playerNum = i;
                return;
            }
        }
    }
    #endregion

    public void TogglePlayerReady()
    {
        Debug.Log($"playerNum : {playerNum}");
        ReadyForGame(playerNum);

        PhotonView pv = PhotonView.Get(this);
        pv.RPC("ReadyForGame", RpcTarget.All, playerNum);
    }

    #region PunRPC
    [PunRPC] void SetTmd(TurnMatchData tmd) => FirebaseManager.turnMatchData = tmd;

    [PunRPC] void ReadyForGame(int playerNum)
    {
        FirebaseManager.gameData.playerReady[playerNum] = !FirebaseManager.gameData.playerReady[playerNum];
        Debug.Log($"player ready : {FirebaseManager.gameData.playerReady[playerNum]}");
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

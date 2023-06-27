using Photon;
using Photon.Pun;
using Photon.Realtime;

public class GamePlayer : MonoBehaviourPunCallbacks
{
    #region Field
    PhotonView pv;
    public static string[] playerNames;

    int playerNum;
    bool isProposer;
    #endregion

    #region MonoBehabiour
    void Awake()
    {
        pv = PhotonView.Get(this);
        // playerNum 지정 필요
        playerNames = new string[PhotonNetwork.PlayerList.Length];
    }

    private void Start()
    {
        pv.RPC("GetPlayerName", RpcTarget.All, playerNum);
    }

    void OnDestroy()
    {
           
    }
    #endregion

    public void SetField(GameData gameData, int playerNum)
    {
        this.playerNum = playerNum;        
    }

    #region Syncronize and State
    [PunRPC] public void GetPlayerName(int playerNum) 
        => playerNames[playerNum] = FirebaseManager.gameData.players[playerNum].playerName;

    [PunRPC] public void SetTmd(TurnMatchData tmd) => FirebaseManager.turnMatchData = tmd;

    [PunRPC] public void ProposeGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, int proposeGold, int roomNum)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gold = proposeGold;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].gameRoomNum = (roomNum == 1) ? true : false;

        FirebaseManager.Instance.SaveGameData();
    }

    [PunRPC] public void GetGoldInTurn(int playerNum, int otherPlayerNum, int turnNum, bool isAchieved)
    {
        FirebaseManager.gameData.players[playerNum].turnData[turnNum].isAchieved = isAchieved;
        FirebaseManager.gameData.players[otherPlayerNum].turnData[turnNum].isAchieved = isAchieved;

        FirebaseManager.Instance.SaveGameData();
    }
    #endregion
}

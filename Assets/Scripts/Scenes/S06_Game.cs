using UnityEngine;
using TMPro;
using System.Linq;
using Photon.Pun;

public class S06_Game : Scenes
{
    #region Field
    PhotonView pv;
    int curPlayer;

    [SerializeField] TMP_Text TurnNumText;  // 몇 번째 턴인지
    [SerializeField] TMP_Text TurnStateText;    // Get vs Set

    /* Game */
    bool isTmdInit = false;
    string[] playerName = new string[4];
    int curTurnNum;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        pv = PhotonView.Get(this);
    }
    void Update()
    {
        ForUpdate();
    }

    void OnDestroy()
    {
        ForOnDestroy();
    }
    #endregion

    #region State Set
    void SetStateText(bool b) => TurnStateText.text = b ? "Get" : "Set";


    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
    #endregion

    #region Set Game
    public void SetGame()
    {
        if(curPlayer == 0)
        {
            FirebaseManager.turnMatchData = new TurnMatchData();
            FirebaseManager.turnMatchData.turn = new Turn[6];
            FirebaseManager.gameData = new GameData();
            SetOpponent();
        }

    }

    void SetOpponent()
    {
        Turn[] turns = InitTurns();
        System.Random random = new System.Random();
        for (int i = 0; i < 3; i++)
        {
            if (random.Next() % 2 == 0)
            {
                FirebaseManager.turnMatchData.turn[i * 2] = turns[i * 4];
                FirebaseManager.turnMatchData.turn[i * 2 + 1] = turns[i * 4 + 1];

            }
            else
            {
                FirebaseManager.turnMatchData.turn[i * 2] = turns[i * 4 + 2];
                FirebaseManager.turnMatchData.turn[i * 2 + 1] = turns[i * 4 + 3];
            }
        }
        FirebaseManager.turnMatchData.turn = FirebaseManager.turnMatchData.turn.OrderBy(x => random.Next()).ToArray();
    }

    Turn[] InitTurns()
    {
        Turn[] turns = new Turn[12];
        turns[0].Room1.getter = 1;
        turns[0].Room1.proposer = 2;
        turns[0].Room2.getter = 3;
        turns[0].Room2.proposer = 4;

        turns[1].Room1.getter = 2;
        turns[1].Room1.proposer = 1;
        turns[1].Room2.getter = 4;
        turns[1].Room2.proposer = 3;
        //----------------------------
        turns[2].Room1.getter = 1;
        turns[2].Room1.proposer = 2;
        turns[2].Room2.getter = 4;
        turns[2].Room2.proposer = 3;

        turns[3].Room1.getter = 2;
        turns[3].Room1.proposer = 1;
        turns[3].Room2.getter = 3;
        turns[3].Room2.proposer = 4;
        //----------------------------
        turns[4].Room1.getter = 1;
        turns[4].Room1.proposer = 3;
        turns[4].Room2.getter = 2;
        turns[4].Room2.proposer = 4;

        turns[5].Room1.getter = 3;
        turns[5].Room1.proposer = 1;
        turns[5].Room2.getter = 4;
        turns[5].Room2.proposer = 2;
        //----------------------------
        turns[6].Room1.getter = 1;
        turns[6].Room1.proposer = 3;
        turns[6].Room2.getter = 4;
        turns[6].Room2.proposer = 2;

        turns[7].Room1.getter = 3;
        turns[7].Room1.proposer = 1;
        turns[7].Room2.getter = 2;
        turns[7].Room2.proposer = 4;
        //----------------------------
        turns[8].Room1.getter = 1;
        turns[8].Room1.proposer = 4;
        turns[8].Room2.getter = 2;
        turns[8].Room2.proposer = 3;

        turns[9].Room1.getter = 4;
        turns[9].Room1.proposer = 1;
        turns[9].Room2.getter = 3;
        turns[9].Room2.proposer = 2;
        //----------------------------
        turns[10].Room1.getter = 1;
        turns[10].Room1.proposer = 4;
        turns[10].Room2.getter = 3;
        turns[10].Room2.proposer = 2;

        turns[11].Room1.getter = 4;
        turns[11].Room1.proposer = 1;
        turns[11].Room2.getter = 2;
        turns[11].Room2.proposer = 3;

        return turns;
    }

    public void SetPlayerMission(int playerNumber, MissionLevel missionLevel)
    {
        System.Random random = new System.Random();
        int tmp = random.Next(1, 10);
        switch (missionLevel)
        {
            case MissionLevel.Low:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.low.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.low.missionNum = tmp;
                break;
            case MissionLevel.Mid:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.mid.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.mid.missionNum = tmp;
                break;
            case MissionLevel.High:
                while (FirebaseManager.gameData.players[playerNumber].playerMission.high.missionNum == tmp)
                {
                    tmp = random.Next(1, 10);
                }
                FirebaseManager.gameData.players[playerNumber].playerMission.high.missionNum = tmp;
                break;
            default: break;
        }
    }
    #endregion

    #region RPC



    #endregion

    #region Game



    public void ProposeGold(int gold)
    {

    }


    #endregion
}
using UnityEngine;
using TMPro;
using System.Linq;

public class S06_Game : Scenes
{
    #region Field
    int curPlayer;

    [SerializeField] TMP_Text TurnNumText;  // 몇 번째 턴인지
    [SerializeField] TMP_Text LeftTime;

    /* Panels */
    [SerializeField] GameObject SuggestPanel;
    [SerializeField] GameObject GetPanel;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject MissionSetPanel;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] GameObject WaitingPanel;


    [SerializeField] GameObject MissionCheckPanel;
    [SerializeField] GameObject QuitBtnPanel;

    /* Game */
    bool isTmdInit = false;
    string[] playerName = new string[4];
    int curTurnNum;
    #endregion

    #region monobehaviour
    void Start()
    {
        InitialSet();
        SetMissionCheckData();
    }
    void Update()
    {
        ForUpdate();
        if(SuggestPanel.activeSelf == true)
        {
            CheckGoldAmount();
        }
    }
    #endregion

    #region State Set
    public void TimerSet(int sec)
    {
        
    }

    #endregion

    #region SceneChange
    public void BackToCustom() => ChangeToScene(3);

    public void BackToRandom() => ChangeToScene(4);
    #endregion

    #region Set Game
    public void SetGame()
    {
        FirebaseManager.gameData = new GameData();
        if (FirebaseManager.MyPlayer.view.ViewID == 1001)
        {
            FirebaseManager.turnMatchData = new TurnMatchData();
            FirebaseManager.turnMatchData.turn = new Turn[6];
            SetOpponent();
            FirebaseManager.MyPlayer.SetTmdIfP1();
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

    #region Toggle Panels
    void ToggleSuggestPanel(bool b) => SuggestPanel.SetActive(b);

    void ToggleGetPanel(bool b) => GetPanel.SetActive(b);

    void ToggleLoadingPanel(bool b) => LoadingPanel.SetActive(b);

    void ToggleMissionSetPanel(bool b) => MissionSetPanel.SetActive(b);

    void ToggleResultPanel(bool b) => ResultPanel.SetActive(b);

    void ToggleWaitingPanel(bool b) => WaitingPanel.SetActive(b);

    void ToggleMissionCheckPanel(bool b) => MissionCheckPanel.SetActive(b);

    void ToggleQuitBtnPanel(bool b) => QuitBtnPanel.SetActive(b);
    #endregion

    #region Suggest Panel
    [SerializeField] TMP_InputField SuggestGoldInputField;
    [SerializeField] GameObject SuggestConfirmPanel;
    [SerializeField] TMP_Text SuggestConfirmTMP;

    void ToggleSuggestConfirmPanel(bool b) => SuggestConfirmPanel.SetActive(b);

    public void SuggestBtn()
    {
        ToggleSuggestConfirmPanel(true);
    }

    void CheckGoldAmount()
    {
        if(SuggestGoldInputField.text.Length > 2)
        {
            if(SuggestGoldInputField.text != "100")
            {
                SuggestGoldInputField.text.Substring(SuggestGoldInputField.text.Length - 2);
            }
        }
    }

    public void ConfirmSuggestYesBtn()
    {

    }

    public void ConfirmSuggestNoBtn()
    {
        ToggleSuggestConfirmPanel(false);
    }

    void SuggestTurnTimeEnd()
    {

    }
    #endregion

    #region GetPanel
    [SerializeField] TMP_Text GetGoldTMP;

    [SerializeField] GameObject GetConfirmPanel;
    [SerializeField] TMP_Text GetConfirmTMP;

    [SerializeField] GameObject OutConfirmPanel;
    [SerializeField] TMP_Text OutConfirmTMP;

    void SetGetGoldTMP()    // initial
    {

    }
    #endregion

    #region LoadingPanel

    #endregion

    #region Mission Set Panel

    #endregion

    #region Result Panel

    #endregion

    #region Waiting Panel

    #endregion

    #region Mission Check Panel
    [SerializeField] TMP_Text[] MissionGold;
    [SerializeField] TMP_Text[] MissionInformation;

    public void MissionCheckBtn() => ToggleMissionCheckPanel(true);

    public void CloseMissionCheck() => ToggleMissionCheckPanel(false);

    public void LookAllMissions()
    {

    }

    void SetMissionCheckData()
    {
        MissionGold[0].text = "50" + "G";   // 숫자 변경 필요
        MissionInformation[0].text = "Information of Mission 1\nTest Text";

        MissionGold[1].text = "150" + "G";   // 숫자 변경 필요
        MissionInformation[1].text = "Information of Mission 2\nTest Text";

        MissionGold[2].text = "270" + "G";   // 숫자 변경 필요
        MissionInformation[2].text = "Information of Mission 3\nTest Text";
    }

    #endregion

    #region Quit Panel
    public void QuitBtn() => ToggleQuitBtnPanel(true);

    public void CancelQuitBtn() => ToggleQuitBtnPanel(false);

    public void ConfirmQuitBtn()
    {
        photonManager.LeaveRoom();
        ChangeToScene(2);
        // AI 대체 추가
    }

    #endregion

    #region Game



    public void ProposeGold(int gold)
    {

    }


    #endregion
}